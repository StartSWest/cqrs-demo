using Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.Application.CurrencyPair.Interfaces
{
    public interface ICurrencyPairStateService
    {
        Task SetCurrencyPairListAsync(CurrencyPairList currencyPairList, CancellationToken cancellationToken);
        Task<CurrencyPairList> GetCurrencyPairListAsync(CancellationToken cancellationToken);
    }
}