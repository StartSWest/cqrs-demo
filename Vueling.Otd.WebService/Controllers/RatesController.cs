using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vueling.Otd.Application.CurrencyPair.Exceptions;
using Vueling.Otd.Application.CurrencyPair.Queries;

namespace Vueling.Otd.WebService.Controllers
{
    [ApiController]
    [Route("api/rates")]
    public class RatesController : ControllerBase
    {
        private readonly ISender _mediator;

        public RatesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            var currencyPairList = await _mediator.Send(new GetCurrencyPairListQuery());
            return Ok(currencyPairList.CurrencyPairs);
        }

        [HttpGet("rate")]
        public async Task<IActionResult> GetRate(string from, string to)
        {
            try
            {
                return Ok(await _mediator.Send(new GetCurrencyPairQuery { From = from, To = to }));
            }
            catch (CurrencyPairNotExistsException)
            {
                return NotFound();
            }
        }
    }
}