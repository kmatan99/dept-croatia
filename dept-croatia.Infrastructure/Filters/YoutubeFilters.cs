namespace dept_croatia.Infrastructure.Filters
{
    public class YoutubeFilters
    {
        public string Type { get; set; } = "video";
        public string Part { get; set; } = "snippet";
        public string Q { get; set; } = string.Empty;
    }
}
