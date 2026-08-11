using System;

namespace AccountingSystem.Desktop
{
    public class VoucherManager
    {
        public void ProcessLocalVoucher(string type, decimal amount, string accountId, string notes)
        {
            Console.WriteLine($"[Voucher] Processing {type} voucher for amount {amount}, account {accountId}, notes: {notes}");
        }

        public void ProcessLocalExpense(decimal amount, string categoryId, string notes)
        {
            Console.WriteLine($"[Expense] Processing expense for amount {amount}, category {categoryId}, notes: {notes}");
        }
    }
}
