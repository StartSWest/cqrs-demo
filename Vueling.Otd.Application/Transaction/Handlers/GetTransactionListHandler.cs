using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vueling.Otd.Application.Transaction.DTOs;
using Vueling.Otd.Application.Transaction.Exceptions;
using Vueling.Otd.Application.Transaction.Interfaces;
using Vueling.Otd.Application.Transaction.Queries;

namespace Vueling.Otd.Application.Transaction.Handlers
{
    internal class GetTransactionListHandler : IRequestHandler<GetTransactionListQuery, TransactionListDTO>
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionStateService _transactionStateService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransactionListHandler> _logger;

        public GetTransactionListHandler(
            ITransactionService transactionService,
            ITransactionStateService transactionStateService,
            IMapper mapper,
            ILogger<GetTransactionListHandler> logger)
        {
            _transactionService = transactionService;
            _transactionStateService = transactionStateService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TransactionListDTO> Handle(GetTransactionListQuery request, CancellationToken cancellationToken)
        {
            TransactionListDTO transactionListDTO;

            try
            {
                var transactionList = await _transactionService.GetTransactionListAsync(cancellationToken);
                transactionListDTO = _mapper.Map<TransactionListDTO>(transactionList);

                try
                {
                    await _transactionStateService.SetTransactionListAsync(transactionList, cancellationToken);
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
                    var transactionList = await _transactionStateService.GetTransactionListAsync(cancellationToken);
                    transactionListDTO = _mapper.Map<TransactionListDTO>(transactionList);
                }
                catch (Exception loadStateEx)
                {
                    // This is the last resource to retrieve data. If this resource fails also an exception is thrown.
                    throw new TransactionListStateException(loadStateEx.Message);
                }
                finally
                {
                    // If primary transactions service fail, the transactions should be retrieved from the state. Let's log first.
                    _logger.LogError(srvEx, srvEx.Message);
                }
            }

            return transactionListDTO;
        }
    }
}