using CustomRateLimiter.Application.Tickets.Queries.GetUserTickets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomRateLimiter.Application
{
    public static class Application
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetUserTicketsQueryHandler).Assembly));
            return services;
        }
    }
}
