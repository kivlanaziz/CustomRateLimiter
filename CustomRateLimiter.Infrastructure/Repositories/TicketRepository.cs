using CustomRateLimiter.Application.Contracts.Persistence;
using CustomRateLimiter.Domain.Entities;

namespace CustomRateLimiter.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            var tickets = new List<Ticket>
            {
                new Ticket { Id = "1", UserId = "A", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "2", UserId = "B", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "3", UserId = "B", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "4", UserId = "C", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "5", UserId = "C", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "6", UserId = "B", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "7", UserId = "A", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "8", UserId = "D", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "9", UserId = "E", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "10", UserId = "C", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "11", UserId = "A", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "12", UserId = "A", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "13", UserId = "D", Date = DateTimeOffset.MinValue },
                new Ticket { Id = "14", UserId = "A", Date = DateTimeOffset.MinValue },
            };

            return Task.FromResult(tickets.Where(t => t.UserId == userId).ToList());
        }
    }
}
