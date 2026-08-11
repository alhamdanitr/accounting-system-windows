using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AccountingSystem.Domain;

namespace AccountingSystem.Data
{
    public class SyncApiClient
    {
        private readonly HttpClient _httpClient;

        public SyncApiClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
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
    }
}
