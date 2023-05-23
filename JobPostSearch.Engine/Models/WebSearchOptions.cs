namespace JobPostSearch.Engine.Models
{
    public class WebSearchOptions
    {
        public static WebSearchOptions Default => new(3, 1);

        public int RetryRequestTotalSeconds { get; }
        public int RetryRequestSleepSeconds { get; }

        public WebSearchOptions(int totalSeconds, int sleepSeconds)
        {
            if (totalSeconds < 1) throw new ArgumentOutOfRangeException(nameof(totalSeconds));
            if (sleepSeconds < 1) throw new ArgumentOutOfRangeException(nameof(sleepSeconds));

            RetryRequestTotalSeconds = totalSeconds;
            RetryRequestSleepSeconds = sleepSeconds;
        }
    }
}
