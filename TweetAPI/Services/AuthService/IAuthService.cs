namespace TweetAPI.Services.AuthService;

public interface IAuthService
{
    User? User { get; set; }
    ResponseDto<User> Register(RegisterDto request);
    ResponseDto<string> Login(LoginDto request);
}
