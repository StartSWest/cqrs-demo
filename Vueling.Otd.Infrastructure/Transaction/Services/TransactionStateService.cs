using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Vueling.Otd.Application.Transaction.Interfaces;
using TransactionModels = Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.Infrastructure.CurrencyPair.Services
{
    public class TransactionStateService : ITransactionStateService
    {
        private readonly string STORE_NAME;
        private readonly string STORE_KEY = "transactions";

        private readonly IConfiguration _configuration;

        public TransactionStateService(IConfiguration configuration)
        {
            _configuration = configuration;
            STORE_NAME = _configuration.GetRequiredSection("StateStore")["StoreName"];
        }

        public async Task SetTransactionListAsync(TransactionModels::TransactionList transactionListDTO, CancellationToken cancellationToken)
        {
            using var client = new DaprClientBuilder().Build();
            await client.SaveStateAsync(STORE_NAME, STORE_KEY, transactionListDTO, cancellationToken: cancellationToken);
        }

        public async Task<TransactionModels::TransactionList> GetTransactionListAsync(CancellationToken cancellationToken)
        {
            using var client = new DaprClientBuilder().Build();
            return await client.GetStateAsync<TransactionModels::TransactionList>(STORE_NAME, STORE_KEY, cancellationToken: cancellationToken);
        }
    }
}