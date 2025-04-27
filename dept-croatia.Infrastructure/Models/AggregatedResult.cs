namespace dept_croatia.Infrastructure.Models
{
    public class AggregatedResult
    {
        public Movie MovieInfo { get; set; } = new();
        public TrailerInfo TrailerInfo { get; set; } = new();
    }
}