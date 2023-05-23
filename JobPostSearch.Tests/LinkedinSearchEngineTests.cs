using JobPostSearch.Engine.Engines;

namespace JobPostSearch.Tests
{
    public class LinkedinSearchEngineTests
    {
        [Fact]
        public void HappyPathTest()
        {
            var engine = new LinkedinJobSearchEngine();

            var result = engine.Search("software developer");

            Assert.NotNull(result);
            Assert.Equal(25, result.ResultsCount);
        }

        [Fact]
        public void AssertThat_SameQuery_DoesNotYieldSameResults()
        {
            var engine = new LinkedinJobSearchEngine();
            const string query = "software developer";

            var first = engine.Search(query);
            var second = engine.Search(query);

            Assert.NotEqual(first.Jobs.First().EntityUrn, second.Jobs.First().EntityUrn);
        }
    }
}
