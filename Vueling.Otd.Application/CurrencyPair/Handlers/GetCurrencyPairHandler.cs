using AutoMapper;
using MediatR;
using Vueling.Otd.Application.CurrencyPair.DTOs;
using Vueling.Otd.Application.CurrencyPair.Exceptions;
using Vueling.Otd.Application.CurrencyPair.Queries;
using Vueling.Otd.Domain.CurrencyPair.Models;

namespace Vueling.Otd.Application.CurrencyPair.Handlers
{
    internal class GetCurrencyPairHandler : IRequestHandler<GetCurrencyPairQuery, CurrencyPairDTO>
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public GetCurrencyPairHandler(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<CurrencyPairDTO> Handle(GetCurrencyPairQuery request, CancellationToken cancellationToken)
        {
            var currencyPairListDTO = await _sender.Send(new GetCurrencyPairListQuery(), cancellationToken);
            var currencyPairList = _mapper.Map<CurrencyPairList>(currencyPairListDTO);
            var currencyGraph = new CurrencyPairGraph(currencyPairList);

            decimal rate = currencyGraph.GetRate(request.From, request.To);
            if (rate < 0)
            {
                throw new CurrencyPairNotExistsException(request.From, request.To);
            }

            return new CurrencyPairDTO { From = request.From, To = request.To, Rate = rate };
        }
    }
}