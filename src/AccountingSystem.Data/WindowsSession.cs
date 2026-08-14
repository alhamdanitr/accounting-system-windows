using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Versioning;

namespace AccountingSystem.Data
{
    [SupportedOSPlatform("windows")]
    public sealed class WindowsSession
    {
        private readonly AuthApiClient _authClient;
        private readonly string _baseUrl;
        private readonly WindowsSecureStore _secureStore = new();
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private LoginResponse? _loginResponse;

        public WindowsSession(string baseUrl)
        {
            _baseUrl = baseUrl;
            _authClient = new AuthApiClient(baseUrl);
            _loginResponse = _secureStore.Load();
        }

        public bool IsAuthenticated => _loginResponse is not null;
        public string? AccessToken => _loginResponse?.AccessToken;
        public string? RefreshToken => _loginResponse?.RefreshToken;
        public string? TenantId => _loginResponse?.User.TenantId;
        public string? DeviceId => _loginResponse?.Device.Id;
        public string? BranchId => _loginResponse?.User.BranchId;
        public LoginUser? User => _loginResponse?.User;

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _authClient.LoginAsync(request);
            if (response is null)
            {
                ClearLocalSession();
                return false;
            }

            SetSession(response);
            return true;
        }

        public async Task<bool> RefreshAsync()
        {
            if (_loginResponse is null || string.IsNullOrWhiteSpace(RefreshToken) || string.IsNullOrWhiteSpace(TenantId)) return false;
            await _refreshLock.WaitAsync();
            try
            {
                // Another request may have refreshed the session while this caller waited.
                if (_loginResponse is null) return false;
                var response = await _authClient.RefreshAsync(new RefreshRequest(TenantId!, RefreshToken!, DeviceId));
                if (response is null)
                {
                    ClearLocalSession();
                    return false;
                }
                SetSession(response);
                return true;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        public SyncApiClient? CreateSyncClient()
        {
            return string.IsNullOrWhiteSpace(AccessToken) ? null : new SyncApiClient(_baseUrl, AccessToken);
        }

        public async Task LogoutAsync()
        {
            if (_loginResponse is not null && !string.IsNullOrWhiteSpace(RefreshToken) && !string.IsNullOrWhiteSpace(TenantId))
            {
                await _authClient.LogoutAsync(new RefreshRequest(TenantId!, RefreshToken!, DeviceId));
            }
            ClearLocalSession();
        }

        private void SetSession(LoginResponse response)
        {
            _loginResponse = response;
            _secureStore.Save(response);
        }

        private void ClearLocalSession()
        {
            _loginResponse = null;
            _secureStore.Clear();
        }
    }
}
