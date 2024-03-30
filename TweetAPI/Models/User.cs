namespace TweetAPI.Models;

public sealed class User
{
    private int id;
    private string username = string.Empty;
    private string passwordHash = string.Empty;

    public required int Id 
    {
        get => id;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(Id));
            id = value;
        }
    }

    public required string Username 
    {
        get => username;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(Username));
            username = value;
        }
    }

    public required string PasswordHash 
    {
        get => passwordHash;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(PasswordHash));
            passwordHash = value;
        }
    }
}
