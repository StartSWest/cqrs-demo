using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using CurrencyPairModels = Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.Infrastructure.CurrencyPair.Services
{
    public class CurrencyPairService : ICurrencyPairService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CurrencyPairService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<CurrencyPairModels::CurrencyPairList> GetCurrencyPairListAsync(CancellationToken cancellationToken)
        {
            var ratesFilename = _configuration.GetRequiredSection("GatewayApi")["RatesFilename"];
            var client = _httpClientFactory.CreateClient("GatewayApiClient");

            var result = await client.GetFromJsonAsync<IList<CurrencyPairModels::CurrencyPair>>(ratesFilename, cancellationToken);
            if (result == null)
            {
                throw new InvalidDataException();
            }

            return new CurrencyPairModels::CurrencyPairList(result);
        }
    }
}