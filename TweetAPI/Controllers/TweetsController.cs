using Microsoft.AspNetCore.Mvc;
using System.Text;
using TweetAPI.Dtos;
using Tweetinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

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

        [HttpPost]
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