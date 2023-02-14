namespace Vueling.Otd.Application.Transaction.DTOs
{
    public class TransactionListDTO
    {
        public IList<TransactionDTO> Transactions { get; private set; }

        public TransactionListDTO(IList<TransactionDTO> transactions)
        {
            Transactions = transactions;
        }
    }
}