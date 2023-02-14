namespace Vueling.Otd.Domain.CurrencyPair.ValueObjects
{
    public class RoundedRate
    {
        private readonly decimal _rate;

        public RoundedRate(decimal rate)
        {
            _rate = rate;
        }

        public decimal HalfToEven()
        {
            return decimal.Round(_rate, 2, MidpointRounding.ToEven);
        }
    }
}