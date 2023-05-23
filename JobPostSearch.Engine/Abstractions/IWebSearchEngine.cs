namespace JobPostSearch.Engine.Abstractions
{
    public interface IWebSearchEngine<TResult>
    {
        TResult Search(string path, Dictionary<string, string> queryParams);
        Task<TResult> SearchAsync(string path, Dictionary<string, string> queryParams);
    }
}
