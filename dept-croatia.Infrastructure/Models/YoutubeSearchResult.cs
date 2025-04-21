public class YouTubeSearchResult
{
    public string Kind { get; set; }
    public string Etag { get; set; }
    public string NextPageToken { get; set; }
    public string PrevPageToken { get; set; }
    public string RegionCode { get; set; }
    public PageInfo PageInfo { get; set; }
    public List<YouTubeSearchItem> Items { get; set; }
}

public class PageInfo
{
    public int TotalResults { get; set; }
    public int ResultsPerPage { get; set; }
}

public class YouTubeSearchItem
{
    public string Kind { get; set; }
    public string Etag { get; set; }
    public string LiveBroadcastContent { get; set; }
    public YouTubeId Id { get; set; }
    public YouTubeSnippet Snippet { get; set; }
}

public class YouTubeId
{
    public string Kind { get; set; }
    public string VideoId { get; set; }
    public string ChannelId { get; set; }
    public string PlaylistId { get; set; }
}

public class YouTubeSnippet
{
    public DateTime PublishedAt { get; set; }
    public string ChannelId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Dictionary<string, YouTubeThumbnail> Thumbnails { get; set; }
    public string ChannelTitle { get; set; }
    public string LiveBroadcastContent { get; set; }
}

public class YouTubeThumbnail
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}