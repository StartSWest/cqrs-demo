using Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.Application.Transaction.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionList> GetTransactionListAsync(CancellationToken cancellationToken);
    }
}