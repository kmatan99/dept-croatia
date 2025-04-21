using dept_croatia.Infrastructure.Contracts;
using dept_croatia.Infrastructure.Extensions;
using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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
            if (filterOptions.ShouldCache())
            {
                return await _memoryCache.GetOrCreateAsync("discover-default", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30);
                    return await SendGetMoviesRequest();
                });
            }
            else 
            {
                return await SendGetMoviesRequest(filterOptions);
            }
        }

        private async Task<MovieSearchResult> SendGetMoviesRequest(MovieDbFilters filterOptions = null)
        {
            var movieDbConfig = _options.Value.MovieDB;
            var endpoint = filterOptions.Query == null ? movieDbConfig.DiscoverMovies : movieDbConfig.SearchMovies;

            var response = await _httpClient.GetWithFiltersAsync(endpoint, filterOptions);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromStream<MovieSearchResult>();
        }
    }
}
