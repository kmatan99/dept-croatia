using System.Reflection.Metadata.Ecma335;

namespace dept_croatia.Infrastructure.Models
{
    public class FilterOptions
    {
        public string? Title { get; set; }
        public string? Language { get; set; } = "en-US";
        public string? SortBy { get; set; } = "popularity.desc";
        public string? WithKeywords { get; set; }
        public int? Year { get; set; }

        public bool ShouldCache()
        {
            if (!string.IsNullOrWhiteSpace(Title))
                return false;

            return !Year.HasValue &&
                string.IsNullOrWhiteSpace(WithKeywords) &&
                string.IsNullOrWhiteSpace(Language) &&
                SortBy == "popularity.desc";
        }
    }
}
