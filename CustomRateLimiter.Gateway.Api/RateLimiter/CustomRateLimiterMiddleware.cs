using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace CustomRateLimiter.Gateway.Api.RateLimiter
{
    public class CustomRateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomRateLimiterMiddleware> _logger;

        private readonly CustomRateOptions _options;

        private static readonly ConcurrentDictionary<string, UserRateLimiterInfo> _requests = new();
        public CustomRateLimiterMiddleware(RequestDelegate next, ILogger<CustomRateLimiterMiddleware> logger, IOptions<CustomRateOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.Request.Query["userId"].ToString();

            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("userId is required");
                return;
            }

            _logger.LogDebug($"[MIDDLEWARE] | User {userId} sent request");
            if (AllowRequest(userId, DateTime.UtcNow))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            }
        }

        private bool AllowRequest(string userId, DateTime request)
        {
            if (_requests.TryGetValue(userId, out var info))
            {
                _logger.LogDebug($"[MIDDLEWARE] | Current Token: {info.Tokens} - Last Refill Time: {info.LastRefill}");

                if (info.TryConsumeToken(_options.RefillPerSecond, _options.MaxToken, request))
                {
                    _logger.LogDebug($"[MIDDLEWARE] | TOKEN REFRESHED - User {userId} Current Token: {info.Tokens} - Last Refill Time: {info.LastRefill}");
                    return true;
                }
                else
                {
                    _logger.LogDebug($"[MIDDLEWARE] | NO TOKEN LEFT - User {userId} Current Token: {info.Tokens} - Last Refill Time: {info.LastRefill}");
                    return false;
                }
            }
            else
            {
                var rateInfo = new UserRateLimiterInfo()
                {
                    LastRefill = request,
                    Tokens = _options.MaxToken
                };

                _requests.TryAdd(userId, rateInfo);
                _logger.LogDebug($"[MIDDLEWARE] | INIT REQUEST LIMITER FOR USER {userId}");
            }

            return true;
        }
    }
}
