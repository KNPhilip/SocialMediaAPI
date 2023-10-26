namespace YoutubeAPI.Dtos
{
    public class VideoDetailsDto
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
        public string? Thumbnail { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
    }
}