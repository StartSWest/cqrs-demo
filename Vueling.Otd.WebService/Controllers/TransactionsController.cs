using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vueling.Otd.Application.Transaction.Exceptions;
using Vueling.Otd.Application.Transaction.Queries;

namespace Vueling.Otd.WebService.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ISender _mediator;

        public TransactionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactionList = await _mediator.Send(new GetTransactionListQuery());
            return Ok(transactionList.Transactions);
        }

        [HttpGet("bySku")]
        public async Task<IActionResult> GetTransactionBySku(string sku)
        {
            try
            {
                return Ok(await _mediator.Send(new GetTransactionListBySkuQuery { Sku = sku }));
            }
            catch (TransactionNotExistsException)
            {
                return NotFound();
            }
        }
    }
}