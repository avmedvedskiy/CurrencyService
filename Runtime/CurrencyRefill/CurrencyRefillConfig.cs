namespace Currency
{
    public class CurrencyRefillConfig
    {
        public int CurrencyId { get; }
        public int RefillAmount { get; }
        public int MaxCount { get; }
        public int RefillTimeInSeconds { get; }

        public CurrencyRefillConfig(int currencyId, int refillAmount, int maxCount, int refillTimeInSeconds)
        {
            CurrencyId = currencyId;
            RefillAmount = refillAmount;
            MaxCount = maxCount;
            RefillTimeInSeconds = refillTimeInSeconds;
        }
    }
}