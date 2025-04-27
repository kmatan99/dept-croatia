using System.Text.Json.Serialization;

namespace dept_croatia.Infrastructure.Models
{
    public class TrailerResponse
    {
        [JsonPropertyName("Results")]
        public List<TrailerInfo> Videos { get; set; } = new List<TrailerInfo>();
    }

    public class TrailerInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public string YoutubeUrl { get; set; } = string.Empty;
    }
}
