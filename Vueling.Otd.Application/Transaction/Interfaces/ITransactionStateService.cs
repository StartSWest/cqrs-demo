using Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.Application.Transaction.Interfaces
{
    public interface ITransactionStateService
    {
        Task SetTransactionListAsync(TransactionList transactionList, CancellationToken cancellationToken);
        Task<TransactionList> GetTransactionListAsync(CancellationToken cancellationToken);
    }
}