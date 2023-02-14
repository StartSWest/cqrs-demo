using Models = Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.UnitTests.CurrencyPair
{
    public class CurrencyPairListUnitTests
    {
        [Fact]
        public void CurrencyPairList_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Models::CurrencyPairList(null));
        }

        [Fact]
        public void CurrencyPairList_ShouldHaveCurrencyPairs()
        {
            var currencyPairs = new List<Models::CurrencyPair> {
                new Models::CurrencyPair("EUR", "CAD", 0.63m),
                new Models::CurrencyPair("CAD", "EUR", 1.59m)
            };

            var cpl = new Models::CurrencyPairList(currencyPairs);

            Assert.NotNull(cpl.CurrencyPairs);
            Assert.Equal(currencyPairs.Count, cpl.CurrencyPairs.Count);
        }
    }
}