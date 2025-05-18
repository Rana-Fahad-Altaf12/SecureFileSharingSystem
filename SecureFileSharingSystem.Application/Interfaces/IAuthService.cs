using SecureFileSharingSystem.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
        Task<ApplicationUser?> GetUserByApiKeyAsync(string apiKey);
    }
}
