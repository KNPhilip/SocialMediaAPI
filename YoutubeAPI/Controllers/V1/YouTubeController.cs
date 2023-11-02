using Microsoft.AspNetCore.Mvc;
using YoutubeAPI.Dtos;
using YoutubeAPI.Services.YTService;

namespace YoutubeAPI.Controllers
{
    public class YouTubeController : ControllerTemplate
    {
        private readonly IYTService _ytService;

        public YouTubeController(IYTService ytService)
        {
            _ytService = ytService;
        }

        [HttpGet]
        public async Task<ActionResult<YouTubeResponseDto>> GetChannelVideos(string? pageToken = null, int maxResults = 10) =>
            HandleResult(await _ytService.GetChannelVideos(pageToken, maxResults));
    }
}