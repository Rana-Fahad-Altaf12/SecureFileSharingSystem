using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecureFileSharingSystem.Application.Interfaces;
using SecureFileSharingSystem.Persistence.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureFileSharingSystem.Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(string email, string password)
        {
            var apiKey = await GenerateUniqueApiKeyAsync();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                ApiKey = apiKey
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ApplicationUser?> GetUserByApiKeyAsync(string apiKey)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
        }
        private async Task<string> GenerateUniqueApiKeyAsync()
        {
            string apiKey;
            do
            {
                apiKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                              .Replace("=", "").Replace("+", "").Replace("/", "");
            }
            while (await _userManager.Users.AnyAsync(u => u.ApiKey == apiKey)); // Requires EF Core

            return apiKey;
        }
    }
}
