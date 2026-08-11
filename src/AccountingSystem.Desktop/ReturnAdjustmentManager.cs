using System;

namespace AccountingSystem.Desktop
{
    public class ReturnAdjustmentManager
    {
        public void ProcessLocalReturn(string invoiceId, string productId, decimal quantity, string reason, bool isCustomerReturn)
        {
            Console.WriteLine($"[Return] Processing return for invoice {invoiceId}, product {productId}, qty {quantity}, reason: {reason}");
        }

        public void ProcessStockTaking(string warehouseId, string productId, decimal actualQty, string reason)
        {
            Console.WriteLine($"[Stock Taking] Warehouse {warehouseId}, Product {productId}, Actual Qty: {actualQty}, Reason: {reason}");
        }
    }
}
