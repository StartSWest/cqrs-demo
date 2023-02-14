namespace Vueling.Otd.Domain.Transaction.Models
{
    public class Transaction
    {
        public string Sku { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Transaction(string sku, decimal amount, string currency)
        {
            Sku = sku;
            Amount = amount;
            Currency = currency;
        }
    }
}