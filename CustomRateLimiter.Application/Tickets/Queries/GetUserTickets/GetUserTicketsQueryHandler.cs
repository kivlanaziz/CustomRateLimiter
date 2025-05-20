using CustomRateLimiter.Application.Contracts.Persistence;
using CustomRateLimiter.Domain.Entities;
using MediatR;

namespace CustomRateLimiter.Application.Tickets.Queries.GetUserTickets
{
    public class GetUserTicketsQueryHandler : IRequestHandler<GetUserTicketsQuery, GetUserTicketsResponse>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetUserTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<GetUserTicketsResponse> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(request.userId);

            return new GetUserTicketsResponse()
            {
                Tickets = tickets,
            };
        }
    }
}
