namespace dept_croatia.Infrastructure.Filters
{
    public class MovieDbFilters
    {
        // Query respresents searching by title
        public string? Query { get; set; }
        public string? Language { get; set; } = "en-US";
        public string? SortBy { get; set; } = "popularity.desc";
        public int? Year { get; set; }
        public int Page { get; set; } = 1;

        public bool UseDiscoverApi()
        {
            if (!string.IsNullOrWhiteSpace(Query))
                return false;

            return !Year.HasValue &&
                Language == "en-US" &&
                SortBy == "popularity.desc";
        }
    }
}
