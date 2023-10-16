using Microsoft.AspNetCore.Mvc;
using TweetAPI.Models;
using TweetAPI.Services.AuthService;

namespace TweetAPI.Controllers
{
    public class AuthController : ControllerTemplate
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id) =>
            id == 1 ? Ok(_authService.user!) : NotFound();

        [HttpPost("register")]
        public ActionResult<User> Register(RegisterDto request) =>
            HandleResult(_authService.Register(request), nameof(GetUser));

        [HttpPost("login")]
        public ActionResult<string> Login(LoginDto request) =>
            HandleResult(_authService.Login(request));
    }
}
