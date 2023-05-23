namespace JobPostSearch.Engine.Helpers
{
    internal static class StringExtensions
    {
        public static string WithQueryString(this string url, Dictionary<string, string> queryParams)
        {
            var queryString = queryParams
                .Select(x => $"{x.Key}={x.Value}")
                .Aggregate(string.Empty, (acc, next) => acc != string.Empty ? $"{acc}&{next}" : next);

            return $"{url}?{queryString}";
        }
    }
}
