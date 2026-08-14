using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AccountingSystem.Data;

namespace AccountingSystem.Desktop
{
    public sealed class BackgroundSyncService : IDisposable
    {
        private readonly WindowsSession _session;
        private readonly WindowsSyncStore _store = new();
        private readonly SemaphoreSlim _runLock = new(1, 1);
        private Timer? _timer;
        private bool _isRunning;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(15);

        public BackgroundSyncService(WindowsSession session)
        {
            _session = session;
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            _timer = new Timer(async _ => await PerformSyncAsync(), null, TimeSpan.Zero, _period);
        }

        public void Enqueue(SyncOperationDto operation)
        {
            _store.Enqueue(new QueuedSyncOperation(operation));
        }

        public async Task<bool> PerformSyncAsync()
        {
            if (!_session.IsAuthenticated || string.IsNullOrWhiteSpace(_session.TenantId) || string.IsNullOrWhiteSpace(_session.DeviceId)) return false;
            if (!await _runLock.WaitAsync(0)) return false;
            try
            {
                var client = _session.CreateSyncClient();
                if (client is null) return false;
                var state = _store.Load();
                if (state.Operations.Count > 0)
                {
                    var response = await client.PushSyncOperationsAsync(
                        _session.TenantId!,
                        _session.DeviceId!,
                        state.Operations.Select(item => item.Operation).ToList());
                    var acknowledged = response.Results.Where(item => item.Status == "SYNCED")
                        .Select(item => item.IdempotencyKey).ToHashSet();
                    var failed = response.Results.Where(item => item.Status != "SYNCED")
                        .ToDictionary(item => item.IdempotencyKey);
                    var remaining = state.Operations
                        .Where(item => !acknowledged.Contains(item.Operation.IdempotencyKey))
                        .Select(item => failed.TryGetValue(item.Operation.IdempotencyKey, out var result)
                            ? item with
                            {
                                Attempts = item.Attempts + 1,
                                LastError = result.ErrorMessage,
                                NextAttemptAt = DateTimeOffset.UtcNow.Add(Backoff(item.Attempts + 1)),
                            }
                            : item)
                        .ToList();
                    _store.Save(new SyncQueueState(remaining, state.Cursor));
                }

                var cursor = state.Cursor;
                SyncPullResponse pull;
                do
                {
                    pull = await client.PullSyncOperationsAsync(_session.TenantId!, _session.DeviceId!, cursor, 100);
                    cursor = pull.NextCursor;
                    _store.Save(new SyncQueueState(_store.Load().Operations, cursor));
                } while (pull.HasMore);
                return true;
            }
            catch (HttpRequestException error) when (error.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return await _session.RefreshAsync();
            }
            catch
            {
                return false;
            }
            finally
            {
                _runLock.Release();
            }
        }

        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
            _timer = null;
            _isRunning = false;
        }

        public void Dispose()
        {
            Stop();
            _runLock.Dispose();
        }

        private static TimeSpan Backoff(int attempt)
        {
            var exponent = Math.Min(attempt, 6);
            return TimeSpan.FromSeconds(Math.Min(15 * (1 << exponent), 900));
        }
    }
}
