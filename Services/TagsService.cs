using Microsoft.EntityFrameworkCore;
using Serilog;
using StackOverflowTags.Database;
using StackOverflowTags.Models;
using ILogger = Serilog.ILogger;

namespace StackOverflowTags.Services
{
    public class TagsService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        
        private const string BaseApiUrl = "https://api.stackexchange.com/2.3/tags?order=desc&sort=popular&site=stackoverflow&pagesize=100";
        
        public TagsService(HttpClient httpClient, AppDbContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "StackOverflowTags");
            _logger = Log.ForContext<TagsService>();
        }

        public async Task<List<Tag>> GetTags(int page, int pageSize, string sortBy, string order)
        {
            if (!order.Equals("desc", StringComparison.OrdinalIgnoreCase) && !order.Equals("asc", StringComparison.OrdinalIgnoreCase))
            {
                LogError(new ArgumentException("Unrecognized parameter.", nameof(order)));
            }
            
            if (!sortBy.Equals("name", StringComparison.OrdinalIgnoreCase) && !sortBy.Equals("percentage", StringComparison.OrdinalIgnoreCase))
            {
                LogError(new ArgumentException("Unrecognized parameter.", nameof(sortBy)));
            }

            var query = _dbContext.Tags.AsQueryable();

            query = sortBy.ToLower() switch
            {
                "percentage" => order.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(t => t.Percentage) : query.OrderBy(t => t.Percentage),
                "name" => order.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                _ => null
            };

            var tags = await query!.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return tags;
        }

        public async Task<List<Tag>> FetchTags()
        {
            var apiTags = new List<ApiTag>();

            for (int i = 1; i <= 10; i++)
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>($"{BaseApiUrl}&page={i}");
                if (response?.Items == null)
                {
                    LogError(new Exception("Got no results from StackOverflow API."));
                }
                apiTags.AddRange(response!.Items);
            }
            var totalTags = apiTags.Sum(t => t.Count);
            var tags = apiTags.Select(t => new Tag
            {
                Name = t.Name,
                Count = t.Count,
                Percentage = (double)t.Count / totalTags * 100
            }).ToList();

            _dbContext.Tags.RemoveRange(_dbContext.Tags);
            await _dbContext.Tags.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();

            return tags;
        }

        private void LogError(Exception e)
        {
            _logger.Error(e, e.Message);
            throw e;
        }
    }
}