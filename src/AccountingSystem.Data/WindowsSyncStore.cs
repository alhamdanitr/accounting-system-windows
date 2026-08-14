using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.Versioning;
using System.Text.Json;

namespace AccountingSystem.Data
{
    public sealed record QueuedSyncOperation(
        SyncOperationDto Operation,
        int Attempts = 0,
        string? LastError = null,
        DateTimeOffset? NextAttemptAt = null);

    public sealed record SyncQueueState(
        List<QueuedSyncOperation> Operations,
        string Cursor = "0");

    [SupportedOSPlatform("windows")]
    public sealed class WindowsSyncStore
    {
        private readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AccountingSystem",
            "sync-queue.bin");
        private readonly object _gate = new();

        public SyncQueueState Load()
        {
            lock (_gate)
            {
                try
                {
                    if (!File.Exists(_path)) return new SyncQueueState(new List<QueuedSyncOperation>());
                    var protectedBytes = File.ReadAllBytes(_path);
                    var plainText = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                    return JsonSerializer.Deserialize<SyncQueueState>(plainText)
                        ?? new SyncQueueState(new List<QueuedSyncOperation>());
                }
                catch
                {
                    return new SyncQueueState(new List<QueuedSyncOperation>());
                }
            }
        }

        public void Save(SyncQueueState state)
        {
            lock (_gate)
            {
                var directory = Path.GetDirectoryName(_path)!;
                Directory.CreateDirectory(directory);
                var plainText = JsonSerializer.SerializeToUtf8Bytes(state);
                var protectedBytes = ProtectedData.Protect(plainText, null, DataProtectionScope.CurrentUser);
                var temporaryPath = _path + ".tmp";
                File.WriteAllBytes(temporaryPath, protectedBytes);
                File.Move(temporaryPath, _path, true);
            }
        }

        public void Enqueue(QueuedSyncOperation operation)
        {
            var state = Load();
            if (state.Operations.Any(item => item.Operation.IdempotencyKey == operation.Operation.IdempotencyKey)) return;
            state.Operations.Add(operation);
            Save(state);
        }
    }
}
