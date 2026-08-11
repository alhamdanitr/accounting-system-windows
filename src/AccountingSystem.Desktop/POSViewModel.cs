using System.Collections.ObjectModel;
using System.Linq;
using AccountingSystem.Domain;

namespace AccountingSystem.Desktop
{
    public class CartItem
    {
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }
        public decimal Total => Product.SalePrice * Quantity;
    }

    public class POSViewModel
    {
        public ObservableCollection<Product> AvailableProducts { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<CartItem> CartItems { get; set; } = new ObservableCollection<CartItem>();

        public decimal SubTotal => CartItems.Sum(i => i.Total);
        public decimal TaxTotal => SubTotal * 0.05m; // 5% ضريبة افتراضية
        public decimal GrandTotal => SubTotal + TaxTotal;

        public void AddToCart(Product product)
        {
            var existing = CartItems.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }
        }

        public void RemoveFromCart(CartItem item)
        {
            CartItems.Remove(item);
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }
}
