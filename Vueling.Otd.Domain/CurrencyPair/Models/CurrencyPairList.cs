namespace Vueling.Otd.Domain.CurrencyPair.Models
{
    public class CurrencyPairList
    {
        public IList<CurrencyPair> CurrencyPairs { get; private set; }

        public CurrencyPairList(IList<CurrencyPair> currencyPairs)
        {
            CurrencyPairs = currencyPairs ?? throw new ArgumentNullException(nameof(currencyPairs));
        }
    }
}