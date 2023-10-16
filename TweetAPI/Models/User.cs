namespace TweetAPI.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
    }
}
