using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TweetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TweetsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("bulk")]
        public IActionResult ScheduleTweets(PostScheduledTweetListDto request)
        {
            List<PostScheduledTweetDto> invalidTweets = new();
            int scheduledCount = 0;

            foreach(PostScheduledTweetDto tweet in request.Tweets)
            {
                TimeSpan delay = tweet.ScheduleFor - DateTime.UtcNow;

                if (delay <= TimeSpan.Zero)
                    invalidTweets.Add(tweet);

                BackgroundJob.Schedule(() => PostTweet(tweet.Adapt<PostTweetDto>()), delay);
                scheduledCount++;
            }

            return Ok(invalidTweets.Any() 
                ? $"{scheduledCount} tweets scheduled successfully,"
                    + $"however, {invalidTweets.Count} tweets had invalid dates and were not scheduled."
                : $"All {scheduledCount} tweets scheduled successfully!");
        }

        [HttpPost]
        public IActionResult ScheduleTweet(PostScheduledTweetDto request)
        {
            TimeSpan delay = request.ScheduleFor - DateTime.UtcNow;

            if (delay > TimeSpan.Zero)
            {
                BackgroundJob.Schedule(() => PostTweet(request.Adapt<PostTweetDto>()), delay);
                return Ok("Tweet scheduled!");
            }
            else return BadRequest("Please enter a valid date and time.");
        }

        [HttpPost]
        [AutomaticRetry(Attempts = 0)]
        public async Task<IActionResult> PostTweet(PostTweetDto request)
        {
            TwitterClient client = new(_config["XConsumerKey"],
                _config["XConsumerSecret"], _config["XAccessToken"], _config["XAccessSecret"]);

            ITwitterResult result = await client.Execute
                .AdvanceRequestAsync(BuildTwitterRequest(request, client));

            return Ok(result.Content);
        }

        private static Action<ITwitterRequest> BuildTwitterRequest(
            PostTweetDto newTweet, TwitterClient client) =>
                (ITwitterRequest request) =>
                {
                    string jsonBody = client.Json.Serialize(newTweet);
                    StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

                    request.Query.Url = "https://api.twitter.com/2/tweets";
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                };
    }
}