namespace Vueling.Otd.Application.Transaction.Exceptions
{
    public class TransactionNotExistsException : Exception
    {
        public string? WithSku { get; private set; }

        public TransactionNotExistsException(string? withSku)
        {
            WithSku = withSku;
        }
    }
}