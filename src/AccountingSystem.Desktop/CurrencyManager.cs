using System;

namespace AccountingSystem.Desktop
{
    public class CurrencyManager
    {
        private string _currentCurrency = "YER";
        private decimal _exchangeRate = 1.0m;

        public void SetCurrency(string currency, decimal rate)
        {
            _currentCurrency = currency;
            _exchangeRate = rate;
            Console.WriteLine($"[Currency Manager] Currency updated to {_currentCurrency} with rate {_exchangeRate}");
        }

        public decimal ConvertToLocal(decimal amount)
        {
            return amount * _exchangeRate;
        }
    }
}
