using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Persistence.Entities
{
    public class FileRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string StoredFileName { get; set; } // Encrypted filename
        public string UploadedByUserId { get; set; }
        public DateTime UploadedAt { get; set; }

        public ApplicationUser UploadedByUser { get; set; }
    }
}
