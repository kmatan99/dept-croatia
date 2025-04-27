using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Extensions;
using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;

namespace dept_croatia.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMovieDBService _movieDBService;

        public SearchService(IMovieDBService movieDBService) 
        {
            _movieDBService = movieDBService;
        }

        public async Task<List<AggregatedResult>> GetSearchResult(MovieDbFilters filters, int page = 1, int pageSize = 10)
        {
            try 
            {
                var movieDbResult = await _movieDBService.GetMovies(filters);

                if (movieDbResult is null)
                    return [];

                var pagedMovies = movieDbResult.Movies.Page(page, pageSize);
                var movieIds = pagedMovies.Select(m => m.MovieId).ToList();
                var associatedVideos = await _movieDBService.GetTrailers(movieIds);

                return MatchMoviesWithTrailers(pagedMovies, associatedVideos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching search results: {ex.Message}");
            }
        }

        private List<AggregatedResult> MatchMoviesWithTrailers(List<Movie> pagedMovies, List<TrailerInfo> trailers)
        {
            var aggregatedResults = new List<AggregatedResult>();

            foreach (var movie in pagedMovies)
            {
                var trailer = trailers.FirstOrDefault(t => t.MovieId == movie.MovieId);

                if (trailer != null)
                {
                    var aggregatedResult = new AggregatedResult
                    {
                        MovieInfo = movie,
                        TrailerInfo = trailer
                    };

                    aggregatedResults.Add(aggregatedResult);
                }
                else
                {
                    var aggregatedResult = new AggregatedResult
                    {
                        MovieInfo = movie,
                        TrailerInfo = new TrailerInfo()
                    };

                    aggregatedResults.Add(aggregatedResult);
                }
            }

            return aggregatedResults;
        }

    }
}
