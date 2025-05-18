using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Domain.DTOs
{
    public class FileDto
    {
        public string Id { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public DateTime UploadedAt { get; set; }
    }
}
