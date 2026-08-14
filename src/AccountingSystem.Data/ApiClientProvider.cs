using System;
using System.Runtime.Versioning;

namespace AccountingSystem.Data
{
    public static class ApiClientProvider
    {
        public const string BaseUrl = "https://accounting-system-backend-production-97e3.up.railway.app/api/v1/";

        public static AuthApiClient GetAuthClient()
        {
            return new AuthApiClient(BaseUrl);
        }

        [SupportedOSPlatform("windows")]
        public static WindowsSession CreateSession()
        {
            return new WindowsSession(BaseUrl);
        }

        public static SyncApiClient GetSyncClient(string? accessToken = null)
        {
            return new SyncApiClient(BaseUrl, accessToken);
        }
    }
}
