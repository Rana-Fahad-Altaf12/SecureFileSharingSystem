using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Persistence.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string FileId { get; set; }
        public string Action { get; set; } // "Download", "Upload", etc.
        public DateTime Timestamp { get; set; }

        public ApplicationUser User { get; set; }
    }
}
