using System;

namespace AccountingSystem.Data
{
    public static class ApiClientProvider
    {
        public const string BaseUrl = "https://accounting-system-backend-production-97e3.up.railway.app/api/v1/";

        public static SyncApiClient GetClient()
        {
            return new SyncApiClient(BaseUrl);
        }
    }
}
