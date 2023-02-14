using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Vueling.Otd.Application.Transaction.Interfaces;
using TransactionModels = Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.Infrastructure.CurrencyPair.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public TransactionService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<TransactionModels::TransactionList> GetTransactionListAsync(CancellationToken cancellationToken)
        {
            var transactionsFilename = _configuration.GetRequiredSection("GatewayApi")["TransactionsFilename"];
            var client = _httpClientFactory.CreateClient("GatewayApiClient");

            var result = await client.GetFromJsonAsync<IList<TransactionModels::Transaction>>(transactionsFilename, cancellationToken);
            if (result == null)
            {
                throw new InvalidDataException();
            }

            return new TransactionModels::TransactionList(result);
        }
    }
}