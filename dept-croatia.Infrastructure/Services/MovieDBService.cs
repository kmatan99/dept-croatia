using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Extensions;
using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace dept_croatia.Infrastructure.Services
{
    public class MovieDBService : IMovieDBService
    {
        private readonly HttpClient _httpClient;

        private readonly IOptions<ApiConfig> _options;
        private readonly IMemoryCache _memoryCache;

        public MovieDBService(HttpClient httpClient, IOptions<ApiConfig> options, IMemoryCache memoryCache) 
        {
            _httpClient = httpClient;
            _options = options;
            _memoryCache = memoryCache;

            _httpClient.BaseAddress = new Uri(options.Value.MovieDB.MovieDBBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.Value.MovieDB.MovieDBApiToken}");
        }

        public async Task<MovieSearchResult?> GetMovies(MovieDbFilters filterOptions)
        {
            var cacheKey = GenerateCacheKey(filterOptions);

            var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(2);

                if (cacheKey == "discover-default")
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30);
                }
                else
                {
                    //Cache specific results only for short time to avoid memory issues
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                }
                return await SendGetMoviesRequest(filterOptions);
            });

            return result;
        }

        private async Task<MovieSearchResult> SendGetMoviesRequest(MovieDbFilters? filterOptions = null)
        {
            var movieDbConfig = _options.Value.MovieDB;
            var endpoint = filterOptions?.Query == null ? movieDbConfig.DiscoverMovies : movieDbConfig.SearchMovies;

            var response = await _httpClient.GetWithFiltersAsync(endpoint, filterOptions);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromStream<MovieSearchResult>();
        }
        public async Task<List<TrailerInfo>> GetTrailers(List<int> movieIds)
        {
            var tasks = new List<Task<TrailerInfo?>>();
            using var semaphore = new SemaphoreSlim(5);

            foreach (var movieId in movieIds)
            {
                await semaphore.WaitAsync(); 

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var trailer = await GetMovieTrailer(movieId);

                        if (trailer != null)
                        {
                            return new TrailerInfo
                            {
                                Key = trailer.Key,
                                Name = trailer.Name,
                                Site = trailer.Site,
                                MovieId = movieId,
                                YoutubeUrl = $"youtube.com/watch?v={trailer.Key}"
                            };
                        }

                        return null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching trailers for movie {movieId}: {ex.Message}");
                        return null;
                    }
                    finally
                    {
                        semaphore.Release(); 
                    }
                }));
            }

            var results = await Task.WhenAll(tasks);

            return results.Where(r => r != null).ToList();
        }

        private async Task<TrailerInfo?> GetMovieTrailer(int movieId)
        {
            var requestUri = _options.Value.MovieDB.Videos.Replace("{movieId}", movieId.ToString());
            var response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromStream<TrailerResponse>();

            if (result?.Videos == null || !result.Videos.Any())
                return null;

            return result.Videos.FirstOrDefault();
        }
        private string GenerateCacheKey(MovieDbFilters filters)
        {
            if (filters.UseDiscoverApi())
                return "discover-default";

            return $"{filters.Query.ToLower()}-{filters.SortBy}-{filters.Language}";
        }
    }
}
