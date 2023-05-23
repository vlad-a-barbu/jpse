namespace JobPostSearch.Engine.Models
{
    public class WebSearchOptions
    {
        public static WebSearchOptions Default => new(3, 1);

        public int RetryRequestTotalSeconds { get; }
        public int RetryRequestSleepSeconds { get; }

        public WebSearchOptions(int retryTotalSeconds, int retrySleepSeconds)
        {
            if (retryTotalSeconds < 1) throw new ArgumentOutOfRangeException(nameof(retryTotalSeconds));
            if (retrySleepSeconds < 1) throw new ArgumentOutOfRangeException(nameof(retrySleepSeconds));

            RetryRequestTotalSeconds = retryTotalSeconds;
            RetryRequestSleepSeconds = retrySleepSeconds;
        }
    }
}
