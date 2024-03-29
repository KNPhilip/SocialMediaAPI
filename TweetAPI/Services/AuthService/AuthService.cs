namespace TweetAPI.Services.AuthService;

public class AuthService : IAuthService
{
    private static IConfiguration? _config;

    public AuthService(IConfiguration config) 
    {
        _config = config;
    }

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

    private static string CreateJWT(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config!["TokenSecret"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescripter = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescripter);


        return tokenHandler.WriteToken(token);
    }
}
