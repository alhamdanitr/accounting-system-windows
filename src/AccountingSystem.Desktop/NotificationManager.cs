using System;

namespace AccountingSystem.Desktop
{
    public class NotificationManager
    {
        public void ShowDesktopAlert(string title, string message)
        {
            Console.WriteLine($"[Desktop Notification] [{title}] {message}");
        }
    }
}
