using Microsoft.AspNetCore.Identity;
using SecureFileSharingSystem.Application.Interfaces;
using SecureFileSharingSystem.Persistence.Entities;

namespace SecureFileSharingSystem.Infrastructure.Security
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiKeyValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsValidAsync(string userId, string apiKey)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && user.ApiKey == apiKey;
        }
    }
}
