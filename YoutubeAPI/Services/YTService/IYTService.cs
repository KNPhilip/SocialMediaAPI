using YoutubeAPI.Dtos;

namespace YoutubeAPI.Services.YTService
{
    public interface IYTService
    {
        Task<ResponseDto<YouTubeResponseDto>> GetChannelVideos(string? pageToken = null, int maxResults = 10);
    }
}