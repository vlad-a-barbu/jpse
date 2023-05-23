namespace JobPostSearch.Engine.Models
{
    public class LinkedinJobSearchResult
    {
        public int ResultsCount => Jobs.Count;
        public IReadOnlyList<Job> Jobs { get; }

        internal LinkedinJobSearchResult(List<Job> jobs)
        {
            Jobs = jobs;
        }
    }

    public class Job
    {
        internal Job() { }

        public string? EntityUrn { get; internal init; }
        public string? SearchId { get; internal init; }
        public string? TrackingId { get; internal init; }
        public string? JobTitle { get; internal init; }
        public string? JobLink { get; internal init; }
        public string? CompanyName { get; internal init; }
        public string? CompanyLink { get; internal init; }
        public string? Location { get; internal init; }
        public string? SalaryInfo { get; internal init; }
        public string? Benefits { get; internal init; }
        public string? PostDate { get; internal init; }
    }
}
