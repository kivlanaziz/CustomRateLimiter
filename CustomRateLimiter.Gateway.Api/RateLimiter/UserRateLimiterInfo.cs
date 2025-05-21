namespace CustomRateLimiter.Gateway.Api.RateLimiter
{
    public class UserRateLimiterInfo
    {
        public double Tokens { get; set; }
        public DateTime LastRefill { get; set; }
        private readonly Lock _userLock = new();

        public bool TryConsumeToken(double refillPerSecond, int maxToken, DateTime requestTime)
        {
            lock (_userLock)
            {
                var timeElapsed = (requestTime - this.LastRefill).TotalSeconds;

                Tokens = Math.Min(maxToken, this.Tokens + timeElapsed * refillPerSecond);

                if (Tokens >= 1)
                {
                    Tokens -= 1;
                    LastRefill = requestTime;

                    return true;
                }

                return false;
            }
        }
    }
}
