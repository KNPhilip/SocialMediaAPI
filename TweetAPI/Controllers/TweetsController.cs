using Microsoft.AspNetCore.Mvc;
using TweetAPI.Services.XService;

namespace TweetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IXService _xService;

        public TweetsController(IXService xService)
        {
            _xService = xService;
        }

        [HttpPost("bulk")]
        public IActionResult ScheduleTweets(PostScheduledTweetListDto request) =>
            Ok(_xService.ScheduleTweets(request));

        [HttpPost]
        public IActionResult ScheduleTweet(PostScheduledTweetDto request) =>
            _xService.ScheduleTweet(request) ? Ok("Tweet scheduled!") : BadRequest("Please enter a valid date and time.");

        [HttpPost]
        public async Task<IActionResult> PostTweet(PostTweetDto request) =>
            Ok(await _xService.PostTweetAsync(request));
    }
}