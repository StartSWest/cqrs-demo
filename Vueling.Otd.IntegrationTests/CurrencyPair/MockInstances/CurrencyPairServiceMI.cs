using Moq;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using Vueling.Otd.Domain.CurrencyPair.Models;
using CPModels = Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.IntegrationTests.CurrencyPair.MockInstances
{
    public class CurrencyPairServiceMI : MockInstanceBase<ICurrencyPairService>
    {
        public override ICurrencyPairService Instance()
        {
            return Mock.Of<ICurrencyPairService>(m =>
                m.GetCurrencyPairListAsync(It.IsAny<CancellationToken>()) ==
                    Task.FromResult(new CurrencyPairList(
                        new List<CPModels::CurrencyPair> {
                            new CPModels::CurrencyPair("EUR", "USD", 1.359m),
                            new CPModels::CurrencyPair("CAD", "EUR", 0.732m),
                            new CPModels::CurrencyPair("USD", "EUR",0.736m),
                            new CPModels::CurrencyPair("EUR", "CAD", 1.366m)
                        })
                    ), MockBehavior.Strict);
        }
    }
}