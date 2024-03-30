namespace YoutubeAPI.Dtos;

public sealed class YouTubeResponseDto
{
    public List<VideoDetailsDto> Videos { get; set; } = [];
    public string? NextPageToken { get; set; }
    public string? PrevPageToken { get; set; }
}
