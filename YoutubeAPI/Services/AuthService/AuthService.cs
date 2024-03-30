namespace YoutubeAPI.Services.AuthService;

public sealed class AuthService(IConfiguration config) : IAuthService
{
    private readonly IConfiguration _config = config;

    public User? User { get; set; }

    public ResponseDto<User> Register(RegisterDto request)
    {
        User returning = (User, request).Adapt<User>();

        returning.Id = 1;

        return ResponseDto<User>.CreatedResponse(returning, returning.Id);
    }
        
    public ResponseDto<string> Login(LoginDto request)
    {
        if (request.Username != User!.Username ||
            !BCrypt.Net.BCrypt.Verify(User.PasswordHash, request.Password))
                return ResponseDto<string>.ErrorResponse("Incorrect username or password.");

        return ResponseDto<string>.SuccessResponse(CreateJWT(User));
    }

    private string CreateJWT(User request)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin")
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["TokenKey"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor scripter = new()
        {
            SigningCredentials = creds,
            Expires = DateTime.UtcNow.AddDays(7),
            Subject = new ClaimsIdentity(claims)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(scripter);

        return tokenHandler.WriteToken(token);
    }
}
