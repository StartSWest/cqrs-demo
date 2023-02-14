using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using Vueling.Otd.Application.CurrencyPair.Queries;
using Vueling.Otd.IntegrationTests.CurrencyPair.MockInstances;

namespace Vueling.Otd.IntegrationTests.CurrencyPair.Queries
{
    [TestFixture]
    public class GetCurrencyPairListQueryTests
    {
        [Test]
        public async Task GetCurrencyPairListQuery_ShouldReturnData()
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

            var result = await mediator.Send(new GetCurrencyPairListQuery());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CurrencyPairs, Is.Not.Null);
            Assert.That(result.CurrencyPairs, Has.Count.EqualTo(4));
        }

        [Test]
        public async Task GetCurrencyPairListQuery_ShouldReturnDataFromCacheOnFail()
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

            var result = await mediator.Send(new GetCurrencyPairListQuery());

            // Second let's simulate another call this time with a failure in the primary
            // service so we force the data to be retrieved from the case.
            var _factory2 = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services
                        .Remove<ICurrencyPairService>()
                        .AddTransient(_ => new CurrencyPairServiceFailMI().Instance());
                });
            });

            var _scopeFactory2 = _factory2.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope2 = _scopeFactory2.CreateScope();

            var mediator2 = scope2.ServiceProvider.GetRequiredService<ISender>();

            var result2 = await mediator2.Send(new GetCurrencyPairListQuery());

            Assert.That(result2, Is.Not.Null);
            Assert.That(result2.CurrencyPairs, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result2.CurrencyPairs, Has.Count.EqualTo(4));
                Assert.That(result.CurrencyPairs, Has.Count.EqualTo(4));
            });
        }
    }
}