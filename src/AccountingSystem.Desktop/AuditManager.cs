using System;

namespace AccountingSystem.Desktop
{
    public class AuditManager
    {
        public void LogAction(string action, string entity, string details)
        {
            var timestamp = DateTime.Now;
            Console.WriteLine($"[Audit Log] [{timestamp}] Action: {action} | Entity: {entity} | Details: {details}");
            // In production, save to local SQLite audit table and sync with central backend
        }
    }
}
