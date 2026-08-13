using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AccountingSystem.Domain;

namespace AccountingSystem.Data
{
    public class SyncApiClient
    {
        private readonly HttpClient _httpClient;

        public SyncApiClient(string baseUrl, string? accessToken = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<List<Product>> GetProductsAsync(string tenantId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Product>>($"products/{tenantId}") ?? new List<Product>();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public async Task<bool> PushSyncOperationsAsync(string tenantId, string deviceId, object operationsPayload)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("sync/push", new
                {
                    tenantId,
                    deviceId,
                    operations = operationsPayload
                });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<JsonElement?> PullSyncOperationsAsync(string tenantId, string deviceId)
        {
            try
            {
                using var response = await _httpClient.GetAsync(
                    $"sync/pull?tenantId={Uri.EscapeDataString(tenantId)}&deviceId={Uri.EscapeDataString(deviceId)}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<JsonElement>();
            }
            catch
            {
                return null;
            }
        }
    }
}
