using HtmlAgilityPack;
using JobPostSearch.Engine.Models;

namespace JobPostSearch.Engine.Engines
{
    public class LinkedinJobSearchEngine : BaseWebSearchEngine<LinkedinJobSearchResult>
    {
        private const string BaseUrl = "https://www.linkedin.com";
        private const string JobSearchGuestPath = "/jobs-guest/jobs/api/seeMoreJobPostings/search";

        private int _offset;

        private static void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri(BaseUrl);
        }

        public LinkedinJobSearchEngine(WebSearchOptions? options = null)
            : base(ConfigureHttpClient, options) { }

        protected override LinkedinJobSearchResult Mapping(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            var items = document.DocumentNode.SelectNodes("//li");

            var jobs = items.Select(x => {
                var node = x.SelectSingleNode("//div");
                return new Job
                {
                    EntityUrn = node.GetAttributeValue("data-entity-urn", string.Empty),
                    SearchId = node.GetAttributeValue("data-search-id", string.Empty),
                    TrackingId = node.GetAttributeValue("data-tracking-id", string.Empty),
                    JobTitle = node.SelectSingleNode(".//h3[@class='base-search-card__title']")?.InnerText.Trim(),
                    JobLink = node.SelectSingleNode(".//a[@class='base-card__full-link']")?.GetAttributeValue("href", string.Empty),
                    CompanyName = node.SelectSingleNode(".//h4[@class='base-search-card__subtitle']//a")?.InnerText.Trim(),
                    CompanyLink = node.SelectSingleNode(".//h4[@class='base-search-card__subtitle']//a")?.GetAttributeValue("href", string.Empty),
                    Location = node.SelectSingleNode(".//span[@class='job-search-card__location']")?.InnerText.Trim(),
                    SalaryInfo = node.SelectSingleNode(".//span[@class='job-search-card__salary-info']")?.InnerText.Trim(),
                    Benefits = node.SelectSingleNode(".//div[@class='result-benefits']//span[@class='result-benefits__text']")?.InnerText.Trim(),
                    PostDate = node.SelectSingleNode(".//time")?.GetAttributeValue("datetime", string.Empty)
                };
            }).ToList();

            return new LinkedinJobSearchResult(jobs);
        }

        public LinkedinJobSearchResult Search(string query)
        {
            var result = Search(JobSearchGuestPath, new Dictionary<string, string>
            {
                ["keywords"] = query,
                ["start"] = _offset.ToString()
            });

            IncreaseOffset(result);

            return result;
        }

        public async Task<LinkedinJobSearchResult> SearchAsync(string query)
        {
            var result = await SearchAsync(JobSearchGuestPath, new Dictionary<string, string>
            {
                ["keywords"] = query,
                ["start"] = _offset.ToString()
            });

            IncreaseOffset(result);

            return result;
        }

        public IReadOnlyList<LinkedinJobSearchResult> ExhaustiveSearch(string query)
        {
            LinkedinJobSearchResult SearchInternal()
            {
                var result = Search(JobSearchGuestPath, new Dictionary<string, string>
                {
                    ["keywords"] = query,
                    ["start"] = _offset.ToString()
                });

                IncreaseOffset(result);

                return result;
            }

            var results = new List<LinkedinJobSearchResult>();

            while (true)
            {
                try { results.Add(SearchInternal()); }
                catch (Exception) { break; }
            }

            return results;
        }

        public async Task<IReadOnlyList<LinkedinJobSearchResult>> ExhaustiveSearchAsync(string query)
        {
            async Task<LinkedinJobSearchResult> SearchInternalAsync()
            {
                var result = await SearchAsync(JobSearchGuestPath, new Dictionary<string, string>
                {
                    ["keywords"] = query,
                    ["start"] = _offset.ToString()
                });

                IncreaseOffset(result);

                return result;
            }

            var results = new List<LinkedinJobSearchResult>();

            while (true)
            {
                try { results.Add(await SearchInternalAsync()); }
                catch (Exception) { break; }
            }

            return results;
        }

        private void IncreaseOffset(LinkedinJobSearchResult result) { _offset += result.ResultsCount; }
    }
}
