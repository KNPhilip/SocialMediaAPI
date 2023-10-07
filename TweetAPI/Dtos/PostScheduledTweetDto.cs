using Newtonsoft.Json;

namespace TweetAPI.Dtos
{
    public class PostScheduledTweetDto
    {
        [JsonProperty("text")]
        public required string Text { get; set; }
        public DateTime ScheduleFor { get; set; }
    }
}
