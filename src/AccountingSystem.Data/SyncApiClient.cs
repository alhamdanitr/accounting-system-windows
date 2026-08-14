using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AccountingSystem.Domain;

namespace AccountingSystem.Data
{
    public sealed record SyncOperationDto(
        string IdempotencyKey,
        string EntityType,
        string EntityId,
        string OperationType,
        string Payload);

    public sealed record SyncPushRequest(
        string TenantId,
        string DeviceId,
        IReadOnlyList<SyncOperationDto> Operations);

    public sealed record SyncOperationResult(
        string IdempotencyKey,
        string OperationId,
        string Status,
        string? Sequence,
        bool? Duplicate,
        bool? Retryable,
        string? ErrorMessage);

    public sealed record SyncPushResponse(
        bool Success,
        int ProcessedCount,
        IReadOnlyList<SyncOperationResult> Results);

    public sealed record RemoteSyncOperation(
        string Id,
        string Sequence,
        string TenantId,
        string DeviceId,
        string EntityType,
        string EntityId,
        string OperationType,
        string Payload,
        string Status);

    public sealed record SyncPullResponse(
        bool Success,
        IReadOnlyList<RemoteSyncOperation> Operations,
        string NextCursor,
        bool HasMore);

    public sealed class SyncApiClient
    {
        private readonly HttpClient _httpClient;

        public SyncApiClient(string baseUrl, string? accessToken = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<List<Product>> GetProductsAsync(string tenantId)
        {
            using var response = await _httpClient.GetAsync($"products/{Uri.EscapeDataString(tenantId)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
        }

        public async Task<SyncPushResponse> PushSyncOperationsAsync(
            string tenantId,
            string deviceId,
            IReadOnlyList<SyncOperationDto> operations)
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "sync/push",
                new SyncPushRequest(tenantId, deviceId, operations));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SyncPushResponse>()
                ?? throw new InvalidOperationException("The sync push response was empty.");
        }

        public async Task<SyncPullResponse> PullSyncOperationsAsync(
            string tenantId,
            string deviceId,
            string cursor = "0",
            int limit = 100)
        {
            using var response = await _httpClient.GetAsync(
                $"sync/pull?tenantId={Uri.EscapeDataString(tenantId)}&deviceId={Uri.EscapeDataString(deviceId)}&cursor={Uri.EscapeDataString(cursor)}&limit={limit}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SyncPullResponse>()
                ?? throw new InvalidOperationException("The sync pull response was empty.");
        }
    }
}
