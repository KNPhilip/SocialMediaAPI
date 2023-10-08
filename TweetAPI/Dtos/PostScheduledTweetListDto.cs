namespace TweetAPI.Dtos
{
    public class PostScheduledTweetListDto
    {
        public List<PostScheduledTweetDto> Tweets { get; set; } = new();
    }
}