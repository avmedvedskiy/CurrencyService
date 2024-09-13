using System;
using System.Threading;
using System.Threading.Tasks;

namespace Currency
{
    public class CurrencyRefill
    {
        public event Action<int> OnCurrencyRefilled; 
        public int CurrencyId => Config.CurrencyId;
        public DateTime NextRefillTime => _nextRefillTime;
        public bool IsInitialized { get; private set; }
        
        private readonly CurrencyService _currencyService;
        private CurrencyRefillConfig Config => _config;
        private CurrencyRefillConfig _config;
        private CancellationTokenSource _tokenSource;
        private DateTime _nextRefillTime;

        public CurrencyRefill(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public void Initialize(CurrencyRefillConfig config)
        {
            _config = config;
            IsInitialized = true;
        }

        public void ProcessRefill()
        {
            if (NeedToRefill())
            {
                if (DateTime.UtcNow > NextRefillTime)
                    SetNextRefillTime();
                StartTimer();
            }
            else
            {
                _nextRefillTime = default;
                Stop();
            }
        }


        public void ProcessRefill(DateTime nextTimeRefill)
        {
            _nextRefillTime = nextTimeRefill;
            if (DateTime.UtcNow > nextTimeRefill && nextTimeRefill != default)
            {
                Stop();
                var time = (DateTime.UtcNow - nextTimeRefill).TotalSeconds;
                var amount = (int)(time / Config.RefillTimeInSeconds);
                _nextRefillTime = DateTime.UtcNow +
                                  TimeSpan.FromSeconds(Config.RefillTimeInSeconds - time % Config.RefillTimeInSeconds);
                if (amount == 0)
                    amount++;
                RefillAmountClamped(amount);
            }

            if (NeedToRefill())
            {
                StartTimer();
            }
            else
            {
                _nextRefillTime = default;
            }
        }

        private async void StartTimer()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                await Task.Delay(NextRefillTime - DateTime.UtcNow, _tokenSource.Token);
                //нужно обновлять таймер перед начиследнием, т.к. вью смотрят на начисление валюты
                SetNextRefillTime();
                RefillAmount();
                if (NeedToRefill())
                    StartTimer();
            }
            catch (Exception)
            {
                _tokenSource = null;
            }
        }

        private void SetNextRefillTime()
        {
            _nextRefillTime = DateTime.UtcNow + TimeSpan.FromSeconds(Config.RefillTimeInSeconds);
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        public bool NeedToRefill() => _currencyService.GetCurrency(Config.CurrencyId) < Config.MaxCount;
        
        public int AmountToRefill() => Config.MaxCount - _currencyService.GetCurrency(Config.CurrencyId);

        private void RefillAmount()
        {
            AddCurrency(Config.RefillAmount);
        }

        private void RefillAmountClamped(int count)
        {
            int current = _currencyService.GetCurrency(Config.CurrencyId);
            int totalAmount = Config.RefillAmount * count;
            if (current + totalAmount > Config.MaxCount)
            {
                AddCurrency(Config.MaxCount - current);
            }
            else
            {
                AddCurrency(totalAmount);
            }
        }

        private void AddCurrency(int amount)
        {
            _currencyService.AddCurrency(Config.CurrencyId, amount);
            OnCurrencyRefilled?.Invoke(amount);
        }
    }
}