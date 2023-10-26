namespace YoutubeAPI.Dtos
{
    public class YouTubeResponseDto
    {
        public List<VideoDetailsDto> Videos { get; set; } = new();
        public string? NextPageToken { get; set; }
        public string? PrevPageToken { get; set; }
    }
}