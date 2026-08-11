using System;
using System.Collections.Generic;

namespace AccountingSystem.Desktop
{
    public class ChartDataPoint
    {
        public string Date { get; set; } = string.Empty;
        public decimal Sales { get; set; }
        public decimal Profit { get; set; }
    }

    public class AnalyticsManager
    {
        public List<ChartDataPoint> GetWeeklySalesTrend()
        {
            var data = new List<ChartDataPoint>();
            var today = DateTime.Today;

            for (int i = 6; i >= 0; i--)
            {
                var dateStr = today.AddDays(-i).ToString("yyyy-MM-dd");
                data.Add(new ChartDataPoint
                {
                    Date = dateStr,
                    Sales = 1200m + (i * 150m),
                    Profit = 400m + (i * 50m)
                });
            }

            return data;
        }
    }
}
