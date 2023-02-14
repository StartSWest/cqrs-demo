using Vueling.Otd.Domain.CurrencyPair.ValueObjects;
using Models = Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.UnitTests.CurrencyPair
{
    public class CurrencyPairGraphUnitTests
    {
        [Fact]
        public void CurrencyPairGraph_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Models::CurrencyPairGraph(null));
        }

        [Fact]
        public void CurrencyPairGraph_ShouldGetCorrectRate()
        {
            var currencyPairs = new List<Models::CurrencyPair> {
                new Models::CurrencyPair("EUR", "CAD", 0.63m),
                new Models::CurrencyPair("CAD", "EUR", 1.59m),
                new Models::CurrencyPair("EUR", "USD", 0.63m),
                new Models::CurrencyPair("USD", "EUR", 1.59m),
                new Models::CurrencyPair("CAD", "AUD", 1.46m),
                new Models::CurrencyPair("AUD", "CAD", 0.68m)
            };

            var cpl = new Models::CurrencyPairList(currencyPairs);
            var cpg = new Models::CurrencyPairGraph(cpl);

            // Invalid pairs
            Assert.StrictEqual(-1m, cpg.GetRate(null, null));
            Assert.StrictEqual(-1m, cpg.GetRate("", ""));
            Assert.StrictEqual(-1m, cpg.GetRate("ANY", "CAD"));
            Assert.StrictEqual(-1m, cpg.GetRate("USD", "ANY"));

            // * EUR to all
            Assert.StrictEqual(1.00m, cpg.GetRate("EUR", "EUR"));
            Assert.StrictEqual(0.63m, cpg.GetRate("EUR", "CAD"));
            Assert.StrictEqual(0.63m, cpg.GetRate("EUR", "USD"));
            // Indirect pairs
            Assert.StrictEqual(new RoundedRate(0.9198m).HalfToEven(), cpg.GetRate("EUR", "AUD"));

            // * CAD to all
            Assert.StrictEqual(1.00m, cpg.GetRate("CAD", "CAD"));
            Assert.StrictEqual(1.59m, cpg.GetRate("CAD", "EUR"));
            Assert.StrictEqual(1.46m, cpg.GetRate("CAD", "AUD"));
            // Indirect pairs
            Assert.StrictEqual(new RoundedRate(1.0017m).HalfToEven(), cpg.GetRate("CAD", "USD"));

            // * USD to all
            Assert.StrictEqual(1.00m, cpg.GetRate("USD", "USD"));
            Assert.StrictEqual(1.59m, cpg.GetRate("USD", "EUR"));
            // Indirect pairs
            Assert.StrictEqual(new RoundedRate(1.0017m).HalfToEven(), cpg.GetRate("USD", "CAD"));
            Assert.StrictEqual(new RoundedRate(1.462482m).HalfToEven(), cpg.GetRate("USD", "AUD"));

            // * AUD to all
            Assert.StrictEqual(1.00m, cpg.GetRate("AUD", "AUD"));
            Assert.StrictEqual(0.68m, cpg.GetRate("AUD", "CAD"));
            // Indirect pairs
            Assert.StrictEqual(new RoundedRate(1.0812m).HalfToEven(), cpg.GetRate("AUD", "EUR"));
            Assert.StrictEqual(new RoundedRate(0.681156m).HalfToEven(), cpg.GetRate("AUD", "USD"));
        }
    }
}