namespace TweetAPI.Dtos;

public sealed class PostTweetDto
{
    [JsonProperty("text")]
    public required string Text { get; set; }
}
