using CMSAPI.Data;
using CMSAPI.Helpers;
using CMSAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMSAPI.Services
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }

            return GenerateToken(user.Username);
        }

        private string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:ValidIssuer"],
                Audience = _configuration["Jwt:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
