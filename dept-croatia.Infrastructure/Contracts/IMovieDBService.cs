using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;

namespace dept_croatia.Infrastructure.Contracts
{
    public interface IMovieDBService
    {
        Task<MovieSearchResult?> GetMovies(MovieDbFilters filterOptions);
        Task<List<TrailerInfo>> GetTrailers(List<int> movieIds);
    }
}
