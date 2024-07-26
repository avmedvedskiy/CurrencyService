namespace Currency
{
    public interface ICurrencyRefillObserver
    {
        void Initialize(CurrencyRefill currencyRefill, CurrencyService currencyService);
    }
}