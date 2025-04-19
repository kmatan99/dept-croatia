namespace dept_croatia.Infrastructure.Models
{
    public class ApiConfig
    {
        public required MovieDB MovieDB { get; set; }
        public required Youtube Youtube { get; set; }
    }

    public class MovieDB
    {
        public string MovieDBBaseUrl { get; set; } = string.Empty;
        public string MovieDBApiToken { get; set; } = string.Empty;

        //Endpoints
        public string DiscoverMovies { get; set; } = string.Empty;
    }

    public class Youtube
    {
        public string YoutubeBaseUrl { get; set; } = string.Empty;
        public string YoutubeApiKey { get; set; } = string.Empty;
    }
}
