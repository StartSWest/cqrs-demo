using Models = Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.UnitTests.Transaction
{
    public class TransactionListUnitTests
    {
        [Fact]
        public void Transaction_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Models::TransactionList(null));
        }

        [Fact]
        public void TransactionList_ShouldHaveTransactions()
        {
            var transactions = new List<Models::Transaction> {
                new Models::Transaction("Q1733", 10.63m, "EUR"),
                new Models::Transaction("A2343", 20.1m, "CAD"),
            };

            var tl = new Models::TransactionList(transactions);

            Assert.NotNull(tl.Transactions);
            Assert.Equal(transactions.Count, tl.Transactions.Count);
        }
    }
}