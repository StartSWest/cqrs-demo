namespace Vueling.Otd.Domain.CurrencyPair.Models
{
    public class CurrencyPair
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public decimal Rate { get; private set; }

        public CurrencyPair(string from, string to, decimal rate)
        {
            From = from;
            To = to;
            Rate = rate;
        }
    }
}