namespace Vueling.Otd.Domain.Transaction.Models
{
    public class TransactionList
    {
        public IList<Transaction> Transactions { get; private set; }

        public TransactionList(IList<Transaction> transactions)
        {
            Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
        }
    }
}