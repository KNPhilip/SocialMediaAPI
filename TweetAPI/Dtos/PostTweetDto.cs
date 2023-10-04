using Newtonsoft.Json;

namespace TweetAPI.Dtos
{
    public class PostTweetDto
    {
        [JsonProperty("text")]
        public required string Text { get; set; }
    }
}
