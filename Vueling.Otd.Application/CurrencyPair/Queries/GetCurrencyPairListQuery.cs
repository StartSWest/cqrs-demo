using MediatR;
using Vueling.Otd.Application.CurrencyPair.DTOs;

namespace Vueling.Otd.Application.CurrencyPair.Queries
{
    public record GetCurrencyPairListQuery : IRequest<CurrencyPairListDTO>;
}