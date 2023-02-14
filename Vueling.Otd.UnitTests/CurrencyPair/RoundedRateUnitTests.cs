using Vueling.Otd.Domain.CurrencyPair.ValueObjects;

namespace Vueling.Otd.UnitTests.CurrencyPair
{
    public class RoundedRateUnitTests
    {
        [Fact]
        public void RoundedRate_ShouldRoundHalfToEvenWithTwoDecimalPlaces()
        {
            Assert.StrictEqual(0.10m, new RoundedRate(0.1046m).HalfToEven());
            Assert.StrictEqual(0.11m, new RoundedRate(0.1051m).HalfToEven());
            Assert.StrictEqual(0.11m, new RoundedRate(0.1058m).HalfToEven());
            Assert.StrictEqual(0.91m, new RoundedRate(0.9128m).HalfToEven());
            Assert.StrictEqual(0.92m, new RoundedRate(0.9198m).HalfToEven());
        }
    }
}