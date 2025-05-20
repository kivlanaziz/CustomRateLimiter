namespace CustomRateLimiter.Gateway.Api.RateLimiter
{
    public class CustomRateOptions
    {
        public int MaxRequest { get; set; }
        public TimeSpan Window { get; set; }
    }
}
