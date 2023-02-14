using Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.Application.CurrencyPair.Interfaces
{
    public interface ICurrencyPairService
    {
        Task<CurrencyPairList> GetCurrencyPairListAsync(CancellationToken cancellationToken);
    }
}