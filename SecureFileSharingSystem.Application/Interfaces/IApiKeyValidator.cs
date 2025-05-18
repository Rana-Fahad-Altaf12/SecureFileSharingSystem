using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Application.Interfaces
{
    public interface IApiKeyValidator
    {
        Task<bool> IsValidAsync(string userId, string apiKey);
    }
}
