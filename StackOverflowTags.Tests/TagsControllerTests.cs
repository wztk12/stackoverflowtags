using StackOverflowTags.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace StackOverflowTags.Tests
{
    public class TagsApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TagsApiIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task GetTags()
        {
            var client = _factory.CreateClient();
            var fetch = await client.GetAsync("/tags/refresh");
            fetch.EnsureSuccessStatusCode();
 
            var request = await client.GetAsync($"/tags?page=1&pageSize=20&sortBy=name&order=asc");
            request.EnsureSuccessStatusCode();
            var tags = await request.Content.ReadFromJsonAsync<List<Tag>>();
            var sortedTags = tags!.OrderBy(x => x.Name);
            Assert.Equal(20, tags!.Count);
            Assert.Equal(tags, sortedTags);

            request = await client.GetAsync($"/tags?page=2&pageSize=50&sortBy=percentage&order=desc");
            tags = await request.Content.ReadFromJsonAsync<List<Tag>>()!;
            sortedTags = tags!.OrderByDescending(x => x.Percentage);
            Assert.Equal(50, tags!.Count);
            Assert.Equal(tags, sortedTags);
        }

        [Fact]
        public async Task GetTags_RecognizesBadParameters()
        {
            var client = _factory.CreateClient();
            var request = await client.GetAsync("/tags?page=1&pageSize=20&sortBy=bad&order=asc");
            Assert.Equal(HttpStatusCode.InternalServerError, request.StatusCode);

            request = await client.GetAsync("/tags?page=1&pageSize=20&sortBy=name&order=bad");
            Assert.Equal(HttpStatusCode.InternalServerError, request.StatusCode);
        }

        [Fact]
        public async Task FetchTags_ShouldSaveTagsToDatabase()
        {
            var client = _factory.CreateClient();
            var request = await client.GetAsync("/tags/refresh");
            request.EnsureSuccessStatusCode();
        }
    }
}