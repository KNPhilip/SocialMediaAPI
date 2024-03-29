namespace TweetAPI.Services.XService;

public interface IXService
{
    string ScheduleTweets(PostScheduledTweetListDto request);
    bool ScheduleTweet(PostScheduledTweetDto request);
    Task<string> PostTweetAsync(PostTweetDto request);
}
