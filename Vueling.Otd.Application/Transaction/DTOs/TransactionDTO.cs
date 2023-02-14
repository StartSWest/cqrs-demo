namespace Vueling.Otd.Application.Transaction.DTOs
{
    public class TransactionDTO
    {
        public string Sku { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public TransactionDTO(string sku, decimal amount, string currency)
        {
            Sku = sku;
            Amount = amount;
            Currency = currency;
        }
    }
}