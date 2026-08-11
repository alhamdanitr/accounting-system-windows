using System;

namespace AccountingSystem.Desktop
{
    public class ReportsViewModel
    {
        public decimal TotalSalesToday { get; set; } = 3450.00m;
        public int TotalInvoicesToday { get; set; } = 42;
        public decimal TotalExpensesToday { get; set; } = 210.00m;
        public decimal NetProfitToday => TotalSalesToday - TotalExpensesToday;

        public void RefreshMetrics(decimal sales, int invoices, decimal expenses)
        {
            TotalSalesToday = sales;
            TotalInvoicesToday = invoices;
            TotalExpensesToday = expenses;
        }
    }
}
