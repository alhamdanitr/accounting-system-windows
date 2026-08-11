using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingSystem.Desktop
{
    public class BackgroundSyncService
    {
        private Timer? _timer;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(15);
        private bool _isRunning = false;

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            
            _timer = new Timer(async _ => await PerformSyncAsync(), null, TimeSpan.Zero, _period);
        }

        private async Task PerformSyncAsync()
        {
            try
            {
                Console.WriteLine($"[Background Sync] Starting background sync at {DateTime.Now}");
                
                // TODO: Call Sync API Client to push offline invoices and pull new inventory updates
                await Task.Delay(1000); // Simulate network sync
                
                Console.WriteLine("[Background Sync] Sync completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Background Sync] Error during synchronization: {ex.Message}");
            }
        }

        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _isRunning = false;
        }
    }
}
