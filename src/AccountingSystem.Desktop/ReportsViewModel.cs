using System;
using System.Threading.Tasks;
using AccountingSystem.Data;

namespace AccountingSystem.Desktop
{
    public sealed class ReportsViewModel
    {
        private readonly WindowsSession _session;

        public DailySalesReportDto? DailySalesReport { get; private set; }
        public bool IsLoading { get; private set; }
        public string? ErrorMessage { get; private set; }

        public ReportsViewModel(WindowsSession session)
        {
            _session = session;
        }

        public async Task LoadDailySalesAsync(string warehouseId, DateTime date)
        {
            if (!_session.IsAuthenticated || string.IsNullOrWhiteSpace(_session.TenantId))
                throw new InvalidOperationException("لا توجد جلسة Windows مصادق عليها");
            if (string.IsNullOrWhiteSpace(warehouseId))
                throw new ArgumentException("يجب تحديد المستودع قبل تحميل التقرير", nameof(warehouseId));

            IsLoading = true;
            ErrorMessage = null;
            try
            {
                var client = _session.CreateSyncClient()
                    ?? throw new InvalidOperationException("تعذر إنشاء عميل API للجلسة الحالية");
                DailySalesReport = await client.GetDailySalesReportAsync(_session.TenantId, warehouseId, date);
            }
            catch (Exception error)
            {
                ErrorMessage = error.Message;
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
