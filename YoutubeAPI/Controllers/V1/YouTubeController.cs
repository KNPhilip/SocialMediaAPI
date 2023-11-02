using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using TweetAPI.Controllers;
using YoutubeAPI.Dtos;

namespace YoutubeAPI.Controllers
{
    public class YouTubeController : ControllerTemplate
    {
        private readonly IConfiguration _config;

        public YouTubeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoDetailsDto>>> GetChannelVideos(string? pageToken = null, int maxResults = 10)
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
            searchRequest.MaxResults = maxResults;
            searchRequest.PageToken = pageToken;

            SearchListResponse searchResponse = await searchRequest.ExecuteAsync();

            List<VideoDetailsDto> videoList = searchResponse.Items.Select(item => new VideoDetailsDto
            {
                Title = item.Snippet.Title,
                Link = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                PublishedAt = item.Snippet.PublishedAtDateTimeOffset
            })
            .OrderByDescending(v => v.PublishedAt)
            .ToList();

            YouTubeResponseDto response = new()
            {
                Videos = videoList,
                NextPageToken = searchResponse.NextPageToken,
                PrevPageToken = searchResponse.PrevPageToken
            };

            return Ok(response);
        } 
    }
}