using CustomRateLimiter.Application.Tickets.Queries.GetUserTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomRateLimiter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetUserTickets(string userId)
        {
            var result = await _mediator.Send(new GetUserTicketsQuery(userId));

            return Ok(result);
        }
    }
}
