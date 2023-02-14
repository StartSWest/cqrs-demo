using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Vueling.Otd.Application.CurrencyPair.Exceptions;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using Vueling.Otd.Application.CurrencyPair.Queries;
using Vueling.Otd.IntegrationTests.CurrencyPair.MockInstances;

namespace Vueling.Otd.IntegrationTests.CurrencyPair.Queries
{
    [TestFixture]
    public class GetCurrencyPairQueryTests
    {
        [Test]
        public async Task GetCurrencyPairQuery_ShouldReturnData()
        {
            var _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services
                        .Remove<ICurrencyPairService>()
                        .AddTransient(_ => new CurrencyPairServiceMI().Instance());
                });
            });

            var _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            var result = await mediator.Send(new GetCurrencyPairQuery { From = "EUR", To = "CAD" });

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.From, Is.EqualTo("EUR"));
                Assert.That(result.To, Is.EqualTo("CAD"));
                Assert.That(result.Rate, Is.EqualTo(1.37m));
            });
        }

        [Test]
        public void GetCurrencyPairQuery_ShouldThrowCurrencyPairNotExistsException()
        {
            // Let's first mock the primary service returning a valid and expected data so
            // it gets saved into the cache.
            var _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services
                        .Remove<ICurrencyPairService>()
                        .AddTransient(_ => new CurrencyPairServiceMI().Instance());
                });
            });

            var _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            Assert.ThrowsAsync(Is.TypeOf<CurrencyPairNotExistsException>(), async () =>
                await mediator.Send(new GetCurrencyPairQuery { From = "", To = "" }));
        }
    }
}
