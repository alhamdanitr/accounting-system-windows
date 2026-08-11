using System;

namespace AccountingSystem.Desktop
{
    public class QuotationManager
    {
        public void SaveLocalQuotation(string customerId, int itemsCount, decimal total)
        {
            Console.WriteLine($"[Quotation] Saving local quotation for customer {customerId}, items: {itemsCount}, total: {total}");
        }

        public void ConvertQuotationToInvoice(string quotationId)
        {
            Console.WriteLine($"[Quotation] Converting quotation {quotationId} to sales invoice on desktop");
        }
    }
}
