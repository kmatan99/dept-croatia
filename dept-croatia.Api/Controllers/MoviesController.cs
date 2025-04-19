using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace dept_croatia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieDBService _movieDBService;

        public MoviesController(IMovieDBService movieDBService) 
        {
            _movieDBService = movieDBService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] FilterOptions filterOptions)
        {
            return Ok(await _movieDBService.GetMovies(filterOptions));
        }
    }
}
