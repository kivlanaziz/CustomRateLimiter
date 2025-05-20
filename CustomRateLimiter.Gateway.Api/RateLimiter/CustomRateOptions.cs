namespace CustomRateLimiter.Gateway.Api.RateLimiter
{
    public class CustomRateOptions
    {
        public int MaxToken { get; set; }
        public double RefillPerSecond { get; set; }
    }
}
