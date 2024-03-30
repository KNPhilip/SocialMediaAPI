namespace YoutubeAPI.Controllers;

public sealed class YouTubeController(IYTService ytService) : ControllerTemplate
{
    private readonly IYTService _ytService = ytService;

    [HttpGet]
    public async Task<ActionResult<YouTubeResponseDto>> GetChannelVideos(string? pageToken = null, int maxResults = 10) =>
        HandleResult(await _ytService.GetChannelVideos(pageToken, maxResults));
}
