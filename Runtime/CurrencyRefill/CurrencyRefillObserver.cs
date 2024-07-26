using UnityEngine;

namespace Currency
{
    public class CurrencyRefillObserver : MonoBehaviour, ICurrencyRefillObserver
    {
        private CurrencyRefill _currencyRefill;
        private CurrencyService _currencyService;

        public void Initialize(CurrencyRefill currencyRefill, CurrencyService currencyService)
        {
            _currencyRefill = currencyRefill;
            _currencyService = currencyService;
            _currencyService.OnCurrencyChanged += OnCurrencyChanged;
        }

        private void OnDestroy()
        {
            _currencyService.OnCurrencyChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(Currency currency)
        {
            if (_currencyRefill.CurrencyId == currency.Id && _currencyRefill.NeedToRefill())
                _currencyRefill.ProcessRefill();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if(_currencyRefill == null)
                return;
            
            if (pauseStatus)
            {
                StopRefill();
            }
            else
            {
                ProcessRefill();
            }

        }

        private void StopRefill()
        {
            if (_currencyRefill.IsInitialized)
                _currencyRefill.Stop();
        }

        private void ProcessRefill()
        {
            if (_currencyRefill.IsInitialized)
                _currencyRefill.ProcessRefill(_currencyRefill.NextRefillTime);
        }
    }
}