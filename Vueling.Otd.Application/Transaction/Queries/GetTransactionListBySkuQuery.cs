﻿using MediatR;
using Vueling.Otd.Application.Transaction.DTOs;

namespace Vueling.Otd.Application.Transaction.Queries
{
    public record GetTransactionListBySkuQuery : IRequest<TransactionListWithTotalAmountDTO>
    {
        public string? Sku { get; init; }
    }
}