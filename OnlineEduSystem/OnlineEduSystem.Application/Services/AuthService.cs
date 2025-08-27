// OnlineEduSystem.Application/Services/AuthService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Application.Responses;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
namespace OnlineEduSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public async Task<AuthResult> RegisterAsync(CreateUserDto dto)
        {
            if (await _users.GetByUserNameAsync(dto.UserName) != null)
                return new() { Success = false, Message = "Kullanıcı zaten kayıtlı." };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = dto.FullName,
                UserName = dto.UserName,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password!)
            };
            await _users.AddAsync(user);
            await _users.SaveChangesAsync();
            return new() { Success = true, Token = GenerateToken(user) };
        }

        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByUserNameAsync(dto.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new() { Success = false, Message = "Geçersiz kullanıcı veya şifre." };

            return new() { Success = true, Token = GenerateToken(user) };
        }

        private string GenerateToken(ApplicationUser user)
        {
            var keyStr = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyStr))
                throw new InvalidOperationException("Jwt:Key is missing in configuration.");

            var issuer = _config["Jwt:Issuer"] ?? "OnlineEduSystemAPI";
            var audience = _config["Jwt:Audience"] ?? "OnlineEduSystemClient";

            var durStr = _config["Jwt:DurationInMinutes"];
            int minutes;
            if (!int.TryParse(durStr, System.Globalization.NumberStyles.Integer,
                              System.Globalization.CultureInfo.InvariantCulture, out minutes))
                minutes = 60;

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}