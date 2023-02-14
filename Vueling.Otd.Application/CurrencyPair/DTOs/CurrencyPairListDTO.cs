namespace Vueling.Otd.Application.CurrencyPair.DTOs
{
    public class CurrencyPairListDTO
    {
        public IList<CurrencyPairDTO> CurrencyPairs { get; private set; }

        public CurrencyPairListDTO(IList<CurrencyPairDTO> currencyPairs)
        {
            CurrencyPairs = currencyPairs;
        }
    }
}