using JobPostSearch.Engine.Abstractions;
using JobPostSearch.Engine.Helpers;
using JobPostSearch.Engine.Models;

namespace JobPostSearch.Engine.Engines
{
    public abstract class BaseWebSearchEngine<TResult> : IWebSearchEngine<TResult>, IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly WebSearchOptions _options;

        public BaseWebSearchEngine(WebSearchOptions? options = null)
        {
            _options = options ?? WebSearchOptions.Default;
            _httpClient = new HttpClient();
        }
        public BaseWebSearchEngine(HttpClient httpClient, WebSearchOptions? options = null)
        {
            _options = options ?? WebSearchOptions.Default;
            _httpClient = httpClient;
        }
        public BaseWebSearchEngine(Action<HttpClient> configureClient, WebSearchOptions? options = null)
            : this(options)
        {
            configureClient.Invoke(_httpClient);
        }

        protected abstract TResult Mapping(string response);

        public TResult Search(string path, Dictionary<string, string> queryParams)
        {
            var url = path.WithQueryString(queryParams);

            var (response, exception) = RequestWithRetry(() => _httpClient.GetAsync(url)).Result;
            if (exception != null) throw exception;

            return Mapping(response!);
        }

        public async Task<TResult> SearchAsync(string path, Dictionary<string, string> queryParams)
        {
            var url = path.WithQueryString(queryParams);

            var (response, exception) = await RequestWithRetry(() => _httpClient.GetAsync(url));
            if (exception != null) throw exception;

            return Mapping(response!);
        }

        protected async Task<(string? Response, Exception? Exception)> RequestWithRetry(Func<Task<HttpResponseMessage>> requestAsync)
        {
            Exception? exception = null;

            var endTime = DateTime.UtcNow.AddSeconds(_options.RetryRequestTotalSeconds);
            while (DateTime.UtcNow <= endTime)
            {
                try
                {
                    var response = (await requestAsync()).EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return (content, null);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Thread.Sleep(_options.RetryRequestSleepSeconds * 1000);
            }

            return (null, exception ?? new ArgumentException("Request failed", nameof(requestAsync)));
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
