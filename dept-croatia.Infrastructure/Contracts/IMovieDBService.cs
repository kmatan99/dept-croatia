using dept_croatia.Infrastructure.Models;

namespace dept_croatia.Infrastructure.Contracts
{
    public interface IMovieDBService
    {
        Task<MovieSearchResult> GetMovies(FilterOptions filterOptions);
    }
}
