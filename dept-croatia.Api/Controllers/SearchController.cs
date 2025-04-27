using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace dept_croatia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService) 
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] MovieDbFilters filters)
        {
            return Ok(await _searchService.GetSearchResult(filters));
        }
    }
}
