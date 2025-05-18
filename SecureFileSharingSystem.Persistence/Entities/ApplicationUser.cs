using Microsoft.AspNetCore.Identity;
using System;

namespace SecureFileSharingSystem.Persistence.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ApiKey { get; set; }
    }
}
