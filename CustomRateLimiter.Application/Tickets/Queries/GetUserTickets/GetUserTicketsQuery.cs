using CustomRateLimiter.Domain.Entities;
using MediatR;

namespace CustomRateLimiter.Application.Tickets.Queries.GetUserTickets
{
    public record GetUserTicketsQuery(string userId) : IRequest<GetUserTicketsResponse>
    {
    }

    public class GetUserTicketsResponse
    {
        public List<Ticket>? Tickets { get; set;}
    }
}
