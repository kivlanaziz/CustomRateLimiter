using CustomRateLimiter.Domain.Entities;

namespace CustomRateLimiter.Application.Contracts.Persistence
{
    public interface ITicketRepository
    {
        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId);
    }
}
