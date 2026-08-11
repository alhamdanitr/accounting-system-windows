using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AccountingSystem.Domain;
using AccountingSystem.Data;

namespace AccountingSystem.Desktop
{
    public class MainViewModel
    {
        private readonly SyncApiClient _apiClient;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public MainViewModel(string apiBaseUrl)
        {
            _apiClient = new SyncApiClient(apiBaseUrl);
        }

        public async Task LoadDataAsync(string tenantId)
        {
            var remoteProducts = await _apiClient.GetProductsAsync(tenantId);
            Products.Clear();
            foreach (var p in remoteProducts)
            {
                Products.Add(p);
            }
        }
    }
}
