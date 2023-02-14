using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vueling.Otd.Application.CurrencyPair.DTOs;
using Vueling.Otd.Application.CurrencyPair.Exceptions;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using Vueling.Otd.Application.CurrencyPair.Queries;

namespace Vueling.Otd.Application.CurrencyPair.Handlers
{
    internal class GetCurrencyPairListHandler : IRequestHandler<GetCurrencyPairListQuery, CurrencyPairListDTO>
    {
        private readonly ICurrencyPairService _currencyPairService;
        private readonly ICurrencyPairStateService _currencyPairStateService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCurrencyPairHandler> _logger;

        public GetCurrencyPairListHandler(
            ICurrencyPairService currencyPairService,
            ICurrencyPairStateService currencyPairStateService,
            IMapper mapper,
            ILogger<GetCurrencyPairHandler> logger)
        {
            _currencyPairService = currencyPairService;
            _currencyPairStateService = currencyPairStateService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CurrencyPairListDTO> Handle(GetCurrencyPairListQuery request, CancellationToken cancellationToken)
        {
            CurrencyPairListDTO currencyPairListDTO;
            try
            {
                var currencyPairList = await _currencyPairService.GetCurrencyPairListAsync(cancellationToken);
                currencyPairListDTO = _mapper.Map<CurrencyPairListDTO>(currencyPairList);

                try
                {
                    await _currencyPairStateService.SetCurrencyPairListAsync(currencyPairList, cancellationToken);
                }
                catch (Exception saveStateEx)
                {
                    // Saving state exception shouldn't break the flow. But it must be logged.
                    _logger.LogError(saveStateEx, saveStateEx.Message);
                }

            }
            catch (Exception srvEx)
            {
                try
                {
                    var currencyPairList = await _currencyPairStateService.GetCurrencyPairListAsync(cancellationToken);
                    currencyPairListDTO = _mapper.Map<CurrencyPairListDTO>(currencyPairList);
                }
                catch (Exception loadStateEx)
                {
                    // This is the last resource to retrieve data. If this resource fails also an exception is thrown.
                    throw new CurrencyPairListStateException(loadStateEx.Message);
                }
                finally
                {
                    // If primary rates service fail, the rates should be retrieved from the state. Let's log first.
                    _logger.LogError(srvEx, srvEx.Message);
                }
            }

            return currencyPairListDTO;
        }
    }
}