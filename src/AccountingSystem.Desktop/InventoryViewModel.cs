using System.Collections.ObjectModel;
using System.Linq;
using AccountingSystem.Domain;

namespace AccountingSystem.Desktop
{
    public class InventoryViewModel
    {
        public ObservableCollection<Product> InventoryItems { get; set; } = new ObservableCollection<Product>();

        public void LoadInventory(System.Collections.Generic.IEnumerable<Product> products)
        {
            InventoryItems.Clear();
            foreach (var p in products)
            {
                InventoryItems.Add(p);
            }
        }

        public ObservableCollection<Product> SearchInventory(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return InventoryItems;

            var filtered = InventoryItems.Where(p => 
                p.ArabicName.Contains(query, System.StringComparison.OrdinalIgnoreCase) || 
                p.Sku.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                (p.Barcode != null && p.Barcode.Contains(query))
            );

            return new ObservableCollection<Product>(filtered);
        }
    }
}
