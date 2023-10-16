using Mapster;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TweetAPI.Models;

namespace TweetAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        public static User? user;
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config) 
        {
            _config = config;
        }

        public ResponseDto<User> Register(RegisterDto request) =>
            ResponseDto<User>.SuccessResponse((user, request).Adapt<User>());
            
        public ResponseDto<string> Login(LoginDto request)
        {
            if (request.Username != user!.Username ||
                !BCrypt.Net.BCrypt.Verify(user.PasswordHash, request.Password))
                    return ResponseDto<string>.ErrorResponse("Incorrect username or password.");

            return ResponseDto<string>.SuccessResponse(CreateJWT(user));
        }

        private string CreateJWT(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["TokenSecret"]!));

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
}
