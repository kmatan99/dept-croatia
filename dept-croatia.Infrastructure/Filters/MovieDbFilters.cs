namespace dept_croatia.Infrastructure.Filters
{
    public class MovieDbFilters
    {
        // Query respresents searching by title
        public string? Query { get; set; }
        public string? Language { get; set; } = "en-US";
        public string? SortBy { get; set; } = "popularity.desc";
        public string? WithKeywords { get; set; }
        public int? Year { get; set; }

        public bool ShouldCache()
        {
            if (!string.IsNullOrWhiteSpace(Query))
                return false;

            return !Year.HasValue &&
                string.IsNullOrWhiteSpace(WithKeywords) &&
                Language == "en-US" &&
                SortBy == "popularity.desc";
        }
    }
}
