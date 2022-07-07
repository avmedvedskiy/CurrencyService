using System;
using System.Collections.Generic;

namespace Currency
{
    public class CurrencyService
    {
        
        public event Action<Currency> OnCurrencyChanged;
        private readonly List<Currency> _currencies = new();

        public void SetCurrency(int id, int count)
        {
            int index = _currencies.FindIndex(x => x.Id == id);
            Currency currency;
            if (index != -1)
            {
                currency = _currencies[index];
                currency.Amount = count;
                _currencies[index] = currency;
            }
            else
            {
                currency = new Currency(id, count);
                _currencies.Add(currency);
            }

            OnCurrencyChanged?.Invoke(currency);
        }

        public int GetCurrency(int id)
        {
            return _currencies.Find(x => x.Id == id).Amount;
        }

        public void AddCurrency(int id, int count)
        {
            int value = GetCurrency(id) + count;
            SetCurrency(id, value);
        }

        public void RemoveCurrency(int id, int count)
        {
            int value = GetCurrency(id) - count;
            SetCurrency(id, value);
        }

        public bool HasCurrency(int id)
        {
            return _currencies.FindIndex(x => x.Id == id) != -1;
        }
    }
}
