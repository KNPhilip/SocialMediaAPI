using YoutubeAPI.Models;
using YoutubeAPI.Dtos;

namespace YoutubeAPI.Services.AuthService
{
    public interface IAuthService
    {
        User? user { get; set; }
        ResponseDto<User> Register(RegisterDto request);
        ResponseDto<string> Login(LoginDto request);
    }
}