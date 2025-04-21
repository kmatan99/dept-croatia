using dept_croatia.Infrastructure.Filters;

namespace dept_croatia.Infrastructure.Contracts
{
    public interface IMovieDBService
    {
        Task<MovieSearchResult?> GetMovies(MovieDbFilters filterOptions);
    }
}
