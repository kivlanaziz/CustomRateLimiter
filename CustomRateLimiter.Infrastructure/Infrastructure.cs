using CustomRateLimiter.Application.Contracts.Persistence;
using CustomRateLimiter.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomRateLimiter.Infrastructure
{
    public static class Infrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITicketRepository, TicketRepository>();
            return services;
        }
    }
}
