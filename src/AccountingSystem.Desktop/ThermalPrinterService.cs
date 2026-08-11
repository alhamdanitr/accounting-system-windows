using System;
using System.Text;
using System.Collections.Generic;

namespace AccountingSystem.Desktop
{
    public class ReceiptItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }

    public class ThermalPrinterService
    {
        public byte[] GenerateEscPosBytes(string companyName, string invoiceNumber, List<ReceiptItemDto> items, decimal grandTotal)
        {
            var buffer = new List<byte>();

            // ESC @ (Initialize printer)
            buffer.AddRange(new byte[] { 0x1B, 0x40 });

            // ESC a 1 (Center alignment)
            buffer.AddRange(new byte[] { 0x1B, 0x61, 0x01 });
            // ESC E 1 (Bold on)
            buffer.AddRange(new byte[] { 0x1B, 0x45, 0x01 });
            
            buffer.AddRange(Encoding.UTF8.GetBytes(companyName + "\n"));
            
            // ESC E 0 (Bold off)
            buffer.AddRange(new byte[] { 0x1B, 0x45, 0x00 });
            buffer.AddRange(Encoding.UTF8.GetBytes($"فاتورة مبيعات: {invoiceNumber}\n"));
            buffer.AddRange(Encoding.UTF8.GetBytes("--------------------------------\n"));

            // ESC a 0 (Left alignment)
            buffer.AddRange(new byte[] { 0x1B, 0x61, 0x00 });

            foreach (var item in items)
            {
                var line = $"{item.Name} x{item.Quantity}  {item.Total}$\n";
                buffer.AddRange(Encoding.UTF8.GetBytes(line));
            }

            buffer.AddRange(Encoding.UTF8.GetBytes("--------------------------------\n"));
            
            // Bold on for total
            buffer.AddRange(new byte[] { 0x1B, 0x45, 0x01 });
            buffer.AddRange(Encoding.UTF8.GetBytes($"الإجمالي النهائي: {grandTotal}$\n\n\n"));
            buffer.AddRange(new byte[] { 0x1B, 0x45, 0x00 });

            // Cut paper
            buffer.AddRange(new byte[] { 0x1D, 0x56, 0x41, 0x10 });

            return buffer.ToArray();
        }
    }
}
