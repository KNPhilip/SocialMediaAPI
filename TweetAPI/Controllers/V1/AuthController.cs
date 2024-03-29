namespace TweetAPI.Controllers;

public sealed class AuthController(IAuthService authService) : ControllerTemplate
{
    private readonly IAuthService _authService = authService;

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id) =>
        id == 1 ? Ok(_authService.User!) : NotFound();

    [HttpPost("register")]
    public ActionResult<User> Register(RegisterDto request) =>
        HandleResult(_authService.Register(request), nameof(GetUser));

    [HttpPost("login")]
    public ActionResult<string> Login(LoginDto request) =>
        HandleResult(_authService.Login(request));
}
