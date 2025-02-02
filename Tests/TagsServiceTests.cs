using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Database;
using StackOverflowTags.Services;
using Xunit;


namespace StackOverflowTags.Tests
{
    public class TagsServiceTests
    {
        private readonly TagsService _tagsService;
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public TagsServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "StackOverflowTagsTestDb")
                .Options;

            _dbContext = new AppDbContext(options);
            _httpClient = new HttpClient();
            _tagsService = new TagsService(_httpClient, _dbContext);
        }


        [Fact]
        public async Task GetTags()
        {
            await _tagsService.FetchTags();

            var tags = await _tagsService.GetTags(1, 20, "name", "asc");
            var sortedTags = tags.OrderBy(x => x.Name);
            
            Assert.Equal(20, tags.Count);
            Assert.Equal(tags, sortedTags);

            tags = await _tagsService.GetTags(2, 50, "percentage", "desc");
            sortedTags = tags.OrderByDescending(x => x.Percentage);
            
            Assert.Equal(50, tags.Count());
            Assert.Equal(tags, sortedTags);
        }

        [Fact]
        public async Task GetTags_RecognizesBadParameters()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _tagsService.GetTags(1, 20, "bad", "asc"));
            await Assert.ThrowsAsync<ArgumentException>(async () => await _tagsService.GetTags(1, 20, "name", "bad"));
        }

        [Fact]
        public async Task FetchTags_ShouldSaveTagsToDatabase()
        {
            var result = await _tagsService.FetchTags();

            Assert.Equal(1000, result.Count);

            var tagsInDb = _dbContext.Tags;
            
            Assert.Equal(1000, tagsInDb.Count());
        }
    }
}