namespace dept_croatia.Infrastructure.Models
{
    public class ApiConfig
    {
        public required MovieDB MovieDB { get; set; }
    }

    public class MovieDB
    {
        public string MovieDBBaseUrl { get; set; } = string.Empty;
        public string MovieDBApiToken { get; set; } = string.Empty;

        //Endpoints
        public string DiscoverMovies { get; set; } = string.Empty;
        public string SearchMovies {  get; set; } = string.Empty;
        public string Videos {  get; set; } = string.Empty;
    }
}
