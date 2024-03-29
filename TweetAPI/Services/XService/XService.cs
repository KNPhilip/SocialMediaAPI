namespace TweetAPI.Services.XService;

public sealed class XService(IConfiguration config) : IXService
{
    private readonly IConfiguration _config = config;

    public string ScheduleTweets(PostScheduledTweetListDto request)
    {
        List<PostScheduledTweetDto> invalidTweets = [];
        int scheduledCount = 0;

        foreach (PostScheduledTweetDto tweet in request.Tweets)
        {
            TimeSpan delay = tweet.ScheduleFor - DateTime.UtcNow;

            if (delay <= TimeSpan.Zero)
                invalidTweets.Add(tweet);

            BackgroundJob.Schedule(() => PostTweetAsync(tweet.Adapt<PostTweetDto>()), delay);
            scheduledCount++;
        }

        return invalidTweets.Count != 0
            ? $"{scheduledCount} tweets scheduled successfully,"
                + $"however, {invalidTweets.Count} tweets had invalid dates and were not scheduled."
            : $"All {scheduledCount} tweets scheduled successfully!";
    }

    public bool ScheduleTweet(PostScheduledTweetDto request)
    {
        TimeSpan delay = request.ScheduleFor - DateTime.UtcNow;

        if (delay > TimeSpan.Zero)
        {
            BackgroundJob.Schedule(() => PostTweetAsync(request.Adapt<PostTweetDto>()), delay);
            return true;
        }

        return false;
    }

    // TODO: Block spam method calls. Should only be
    // able to be called once by the other methods.
    public async Task<string> PostTweetAsync(PostTweetDto request)
    {
        TwitterClient client = new(_config["XConsumerKey"],
            _config["XConsumerSecret"], _config["XAccessToken"], _config["XAccessSecret"]);

        ITwitterResult result = await client.Execute
            .AdvanceRequestAsync(BuildTwitterRequest(request, client));

        return result.Content;
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
