namespace Vueling.Otd.Application.Transaction.DTOs
{
    public class TransactionListWithTotalAmountDTO
    {
        public IList<TransactionDTO> Transactions { get; private set; }
        public decimal TotalAmount { get; private set; }

        public TransactionListWithTotalAmountDTO(IList<TransactionDTO> transactions, decimal totalAmount)
        {
            Transactions = transactions;
            TotalAmount = totalAmount;
        }
    }
}