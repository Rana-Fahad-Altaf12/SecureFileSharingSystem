using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Domain.DTOs
{
    public class FileDownloadResult
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
    }
}
