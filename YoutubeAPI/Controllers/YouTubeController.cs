using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;

namespace YoutubeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YouTubeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public YouTubeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetChannelVideos()
        {
            YouTubeService youtubeService = new(new BaseClientService.Initializer
            {
                ApiKey = _config["YTAPIKey"],
                ApplicationName = "YoutubeAPI"
            });

            SearchResource.ListRequest searchRequest =
                youtubeService.Search.List("snippet");
            searchRequest.ChannelId = "";
            searchRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            SearchListResponse searchResponse = await searchRequest.ExecuteAsync();

            return Ok(searchResponse);
        } 
    }
}