using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AccountingSystem.Data;
using AccountingSystem.Domain;

namespace AccountingSystem.Desktop
{
    public sealed class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }
        public decimal Total => Product.SalePrice * Quantity;

        public CartItem(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;
        }

        public void Increase() => Quantity++;
        public void Decrease() => Quantity--;
    }

    public sealed class POSViewModel
    {
        private readonly WindowsSession _session;
        private readonly BackgroundSyncService _backgroundSync;

        public ObservableCollection<WarehouseDto> Warehouses { get; } = new();
        public ObservableCollection<Product> AvailableProducts { get; } = new();
        public ObservableCollection<CartItem> CartItems { get; } = new();
        public WarehouseDto? SelectedWarehouse { get; private set; }
        public decimal SubTotal => CartItems.Sum(item => item.Total);
        public decimal TaxTotal => CartItems.Sum(item => item.Product.SalePrice * item.Quantity * item.Product.TaxRate / 100m);
        public decimal GrandTotal => SubTotal + TaxTotal;

        public POSViewModel(WindowsSession session, BackgroundSyncService backgroundSync)
        {
            _session = session;
            _backgroundSync = backgroundSync;
        }

        public async Task LoadAsync()
        {
            if (!_session.IsAuthenticated || string.IsNullOrWhiteSpace(_session.TenantId))
                throw new InvalidOperationException("لا توجد جلسة Windows مصادق عليها");

            var client = _session.CreateSyncClient()
                ?? throw new InvalidOperationException("تعذر إنشاء عميل API للجلسة الحالية");
            var warehouses = await client.GetWarehousesAsync(_session.TenantId);
            Warehouses.Clear();
            foreach (var warehouse in warehouses) Warehouses.Add(warehouse);
            await SelectWarehouseAsync(warehouses.FirstOrDefault());
        }

        public async Task SelectWarehouseAsync(WarehouseDto? warehouse)
        {
            SelectedWarehouse = warehouse;
            AvailableProducts.Clear();
            CartItems.Clear();
            if (warehouse is null || string.IsNullOrWhiteSpace(_session.TenantId)) return;

            var client = _session.CreateSyncClient()
                ?? throw new InvalidOperationException("تعذر إنشاء عميل API للجلسة الحالية");
            var products = await client.GetProductsForWarehouseAsync(_session.TenantId, warehouse.Id);
            foreach (var product in products.Where(product => product.Active)) AvailableProducts.Add(product);
        }

        public void AddToCart(Product product)
        {
            if (product.CurrentStock <= 0) return;
            var existing = CartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existing is not null)
            {
                if (existing.Quantity < product.CurrentStock) existing.Increase();
                return;
            }
            CartItems.Add(new CartItem(product));
        }

        public void RemoveFromCart(CartItem item) => CartItems.Remove(item);
        public void ClearCart() => CartItems.Clear();

        public bool EnqueueSale(out string message)
        {
            if (!_session.IsAuthenticated || string.IsNullOrWhiteSpace(_session.TenantId) || string.IsNullOrWhiteSpace(_session.DeviceId))
            {
                message = "لا توجد جلسة مصادق عليها لإنشاء الفاتورة.";
                return false;
            }
            if (SelectedWarehouse is null)
            {
                message = "يجب اختيار مستودع قبل إتمام البيع.";
                return false;
            }
            if (CartItems.Count == 0)
            {
                message = "السلة فارغة.";
                return false;
            }

            var saleId = Guid.NewGuid().ToString();
            var payload = JsonSerializer.Serialize(new
            {
                tenantId = _session.TenantId,
                branchId = _session.BranchId,
                warehouseId = SelectedWarehouse.Id,
                userId = _session.User?.Id,
                paymentType = "CASH",
                paidAmount = GrandTotal,
                items = CartItems.Select(item => new
                {
                    productId = item.Product.Id,
                    quantity = (decimal)item.Quantity,
                    unitPrice = item.Product.SalePrice,
                    discount = 0m,
                }).ToList(),
            });

            _backgroundSync.Enqueue(new SyncOperationDto(
                $"sale:{saleId}",
                "SALE",
                saleId,
                "CREATE",
                payload));
            ClearCart();
            message = "تم حفظ الفاتورة محليًا وستتم مزامنتها تلقائيًا.";
            return true;
        }
    }
}
