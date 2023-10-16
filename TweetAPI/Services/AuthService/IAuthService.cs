using TweetAPI.Models;

namespace TweetAPI.Services.AuthService
{
    public interface IAuthService
    {
        User? user { get; set; }
        ResponseDto<User> Register(RegisterDto request);
        ResponseDto<string> Login(LoginDto request);
    }
}
