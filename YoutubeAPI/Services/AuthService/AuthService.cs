using Mapster;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YoutubeAPI.Dtos;
using YoutubeAPI.Models;

namespace YoutubeAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public User? user { get; set; }

        public ResponseDto<User> Register(RegisterDto request)
        {
            User returning = (user, request).Adapt<User>();

            returning.Id = 1;

            return ResponseDto<User>.CreatedResponse(returning, returning.Id);
        }
            
        public ResponseDto<string> Login(LoginDto request)
        {
            if (request.Username != user!.Username ||
                !BCrypt.Net.BCrypt.Verify(user.PasswordHash, request.Password))
                    return ResponseDto<string>.ErrorResponse("Incorrect username or password.");

            return ResponseDto<string>.SuccessResponse(CreateJWT(user));
        }

        private string CreateJWT(User request)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

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
}