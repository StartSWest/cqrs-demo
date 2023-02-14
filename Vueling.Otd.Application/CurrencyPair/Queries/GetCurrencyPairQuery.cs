using MediatR;
using Vueling.Otd.Application.CurrencyPair.DTOs;

namespace Vueling.Otd.Application.CurrencyPair.Queries
{
    public record GetCurrencyPairQuery : IRequest<CurrencyPairDTO>
    {
        public string? From { get; init; }
        public string? To { get; init; }
    }
}