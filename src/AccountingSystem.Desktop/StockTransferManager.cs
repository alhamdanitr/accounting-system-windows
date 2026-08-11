using System;
using System.Collections.Generic;

namespace AccountingSystem.Desktop
{
    public class StockTransferManager
    {
        public void ProcessLocalTransfer(string fromWarehouse, string toWarehouse, string productId, decimal quantity, List<string> serials)
        {
            Console.WriteLine($"[Stock Transfer] Moving {quantity} of product {productId} from {fromWarehouse} to {toWarehouse}");
            if (serials != null && serials.Count > 0)
            {
                Console.WriteLine($"[Serial Tracking] Transferred serials: {string.Join(", ", serials)}");
            }
            // Save to local SQLite queue for sync
        }
    }
}
