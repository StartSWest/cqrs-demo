namespace Vueling.Otd.Application.CurrencyPair.Exceptions
{
    public class CurrencyPairNotExistsException : Exception
    {
        public string? From { get; private set; }
        public string? To { get; private set; }

        public CurrencyPairNotExistsException(string? from, string? to)
        {
            From = from;
            To = to;
        }
    }
}