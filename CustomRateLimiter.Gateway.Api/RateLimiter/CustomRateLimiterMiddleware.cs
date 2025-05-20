using Microsoft.Extensions.Options;

namespace CustomRateLimiter.Gateway.Api.RateLimiter
{
    public class CustomRateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomRateLimiterMiddleware> _logger;

        private readonly CustomRateOptions _options;

        private static readonly Dictionary<string, (int Count, DateTime ResetTime)> _requests = new();
        private static readonly Lock _lock = new();
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
            lock (_lock)
            {
                if (_requests.TryGetValue(userId, out var info))
                {
                    _logger.LogDebug($"[MIDDLEWARE] | Request Count: {info.Count} - Reset Time: {info.ResetTime}");
                    if (request > info.ResetTime)
                    {
                        _requests[userId] = (1, request.Add(_options.Window));
                        _logger.LogDebug($"[MIDDLEWARE] | RESET LIMITER FOR USER {userId}");
                    }
                    else if (info.Count >= _options.MaxRequest)
                    {
                        return false;
                    }
                    else
                    {
                        _requests[userId] = (info.Count + 1, info.ResetTime);
                    }
                }
                else
                {
                    _requests[userId] = (1, request.Add(_options.Window));
                    _logger.LogDebug($"[MIDDLEWARE] | INIT REQUEST LIMITER FOR USER {userId}");
                }

                return true;
            }
        }
    }
}
