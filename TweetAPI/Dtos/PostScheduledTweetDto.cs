namespace TweetAPI.Dtos;

public sealed class PostScheduledTweetDto
{
    [JsonProperty("text")]
    public required string Text { get; set; }
    public DateTime ScheduleFor { get; set; }
}
