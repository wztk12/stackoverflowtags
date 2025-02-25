using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.Models;
using StackOverflowTags.Services;

namespace StackOverflowTags.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagsService _tagsService;
        private readonly ILogger _logger;

        public TagsController(TagsService tagsService, ILogger logger)
        {
            _tagsService = tagsService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of tags from the database.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of tags per page</param>
        /// <param name="sortBy">Sorting option (available: "name" - by name, "percentage" - by percentage share)</param>
        /// <param name="order">Sorting order (available: "asc" - from smallest to largest, "desc" - from largest to smallest)</param>
        /// <returns>A list of tags sorted according to the specified options.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "name",
            [FromQuery] string order = "asc")
        {
            try
            {
                var tags = await _tagsService.GetTags(page, pageSize, sortBy, order);
                return Ok(tags);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Fetches and saves tags from the Stack Overflow API to the database.
        /// </summary>
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTags()
        {
            var tags = await _tagsService.FetchTags();
            return Ok(tags);
        }
    }
}