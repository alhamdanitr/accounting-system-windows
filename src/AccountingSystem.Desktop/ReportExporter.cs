using System;
using System.IO;
using System.Text;

namespace AccountingSystem.Desktop
{
    public class ReportExporter
    {
        public void ExportReportToFile(string reportName, string content, bool isExcel)
        {
            var extension = isExcel ? "csv" : "txt";
            var fileName = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            File.WriteAllText(filePath, content, Encoding.UTF8);
            Console.WriteLine($"[Report Exporter] Report successfully exported to: {filePath}");
        }
    }
}
