using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace dept_croatia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieDBService _movieDBService;
        private readonly IYoutubeService _youtubeService;

        public MoviesController(IMovieDBService movieDBService, IYoutubeService youtubeService) 
        {
            _movieDBService = movieDBService;
            _youtubeService = youtubeService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] MovieDbFilters filterOptions)
        {
            return Ok(await _movieDBService.GetMovies(filterOptions));
        }

        [HttpGet]
        [Route("detail/{movieId}")]
        public async Task<IActionResult> GetMovieDetails(int movieId)
        {
            return Ok();
        }

        [HttpGet]
        [Route("youtube")]
        public async Task<IActionResult> GetYoutube()
        {
            return Ok(await _youtubeService.TestMethod());
        }
    }
}
