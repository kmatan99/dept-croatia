using dept_croatia.Infrastructure.Extensions;
using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.IO;

namespace dept_croatia.Infrastructure.Services
{
    public class YoutubeService : IYoutubeService
    {
        private readonly HttpClient _httpClient;

        private readonly IOptions<ApiConfig> _options;

        public YoutubeService(HttpClient httpClient, IOptions<ApiConfig> options, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _options = options;

            _httpClient.BaseAddress = new Uri(options.Value.Youtube.YoutubeBaseUrl);
        }

        public async Task<YouTubeSearchResult> TestMethod()
        {
            try 
            {
                var filters = new YoutubeFilters();

                filters.Q = "Peaky Blinders";
                //var url = `https://www.youtube.com/watch?v=${result.id.videoId}`;
                var response = await _httpClient.GetWithFiltersAsync(_options.Value.Youtube.Search, 
                    filters, 
                    _options.Value.Youtube.YoutubeApiKey);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromStream<YouTubeSearchResult>();
            }
            catch (Exception ex)
            {
                return new YouTubeSearchResult();
            }
        }
    }
}
