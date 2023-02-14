using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using CurrencyPairModels = Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.Infrastructure.CurrencyPair.Services
{
    public class CurrencyPairStateService : ICurrencyPairStateService
    {
        private readonly string STORE_NAME;
        private readonly string STORE_KEY = "rates";

        private readonly IConfiguration _configuration;

        public CurrencyPairStateService(IConfiguration configuration)
        {
            _configuration = configuration;
            STORE_NAME = _configuration.GetRequiredSection("StateStore")["StoreName"];
        }

        public async Task SetCurrencyPairListAsync(CurrencyPairModels::CurrencyPairList currencyPairListDTO, CancellationToken cancellationToken)
        {
            using var client = new DaprClientBuilder().Build();
            await client.SaveStateAsync(STORE_NAME, STORE_KEY, currencyPairListDTO, cancellationToken: cancellationToken);
        }

        public async Task<CurrencyPairModels::CurrencyPairList> GetCurrencyPairListAsync(CancellationToken cancellationToken)
        {
            using var client = new DaprClientBuilder().Build();
            return await client.GetStateAsync<CurrencyPairModels::CurrencyPairList>(STORE_NAME, STORE_KEY, cancellationToken: cancellationToken);
        }
    }
}