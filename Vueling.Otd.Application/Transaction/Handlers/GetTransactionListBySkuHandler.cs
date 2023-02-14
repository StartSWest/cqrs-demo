using AutoMapper;
using MediatR;
using Vueling.Otd.Application.CurrencyPair.Queries;
using Vueling.Otd.Application.Transaction.DTOs;
using Vueling.Otd.Application.Transaction.Exceptions;
using Vueling.Otd.Application.Transaction.Queries;
using Vueling.Otd.Domain.CurrencyPair.Models;
using Vueling.Otd.Domain.CurrencyPair.ValueObjects;

namespace Vueling.Otd.Application.Transaction.Handlers
{
    internal class GetTransactionListBySkuHandler : IRequestHandler<GetTransactionListBySkuQuery, TransactionListWithTotalAmountDTO>
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public GetTransactionListBySkuHandler(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<TransactionListWithTotalAmountDTO> Handle(GetTransactionListBySkuQuery request, CancellationToken cancellationToken)
        {
            var currencyPairListDTO = await _sender.Send(new GetCurrencyPairListQuery(), cancellationToken);
            var currencyPairList = _mapper.Map<CurrencyPairList>(currencyPairListDTO);
            var currencyGraph = new CurrencyPairGraph(currencyPairList);

            var transactionListDTO = await _sender.Send(new GetTransactionListQuery(), cancellationToken);

            var filteredEURTransactions =
                (from t in transactionListDTO.Transactions
                 where t.Sku == request.Sku
                 let amount = new RoundedRate(t.Amount * currencyGraph.GetRate(t.Currency, "EUR")).HalfToEven()
                 select new TransactionDTO(t.Sku, amount, "EUR"))
                 .ToList();

            if (filteredEURTransactions.Count == 0)
            {
                throw new TransactionNotExistsException(request.Sku);
            }

            var totalEURAmount = filteredEURTransactions.Sum(ft => ft.Amount);

            return new TransactionListWithTotalAmountDTO(filteredEURTransactions, totalEURAmount);
        }
    }
}