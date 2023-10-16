using TweetAPI.Models;

namespace TweetAPI.Services.AuthService
{
    public interface IAuthService
    {
        ResponseDto<User> Register(RegisterDto request);
        ResponseDto<string> Login(LoginDto request);
    }
}
