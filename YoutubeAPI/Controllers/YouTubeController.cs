using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using YoutubeAPI.Dtos;

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

            var videoList = searchResponse.Items.Select(item => new VideoDetailsDto
            {
                Title = item.Snippet.Title,
                Link = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                PublishedAt = item.Snippet.PublishedAtDateTimeOffset
            })
            .OrderByDescending(v => v.PublishedAt)
            .ToList();

            return Ok(searchResponse);
        } 
    }
}