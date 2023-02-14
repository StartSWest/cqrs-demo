using Moq;
using Vueling.Otd.Application.CurrencyPair.Interfaces;

namespace Vueling.Otd.IntegrationTests.CurrencyPair.MockInstances
{
    public class CurrencyPairServiceFailMI : MockInstanceBase<ICurrencyPairService>
    {
        public override ICurrencyPairService Instance()
        {
            var mock = new Mock<ICurrencyPairService>(MockBehavior.Strict);
            mock.Setup(m => m.GetCurrencyPairListAsync(It.IsAny<CancellationToken>()))
                .Throws<Exception>();

            return mock.Object;
        }
    }
}