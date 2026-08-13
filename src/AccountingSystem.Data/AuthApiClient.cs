using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AccountingSystem.Data
{
    public sealed record LoginRequest(
        string TenantId,
        string Identifier,
        string Password,
        string DeviceName,
        string DevicePlatform,
        string DeviceKeyHash);

    public sealed record LoginResponse(
        string AccessToken,
        string TokenType,
        LoginDevice Device,
        LoginUser User);

    public sealed record LoginDevice(
        string Id,
        string Name,
        string Platform);

    public sealed record LoginUser(
        string Id,
        string FullName,
        string Email,
        string TenantId,
        string? BranchId);

    public sealed class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                using var response = await _httpClient.PostAsJsonAsync("auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            catch
            {
                return null;
            }
        }
    }
}
