using Microsoft.AspNetCore.Http;
using SecureFileSharingSystem.Domain.DTOs;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string userId);
        Task<FileDownloadResult> DownloadAsync(string fileId, string userId);
        Task<List<FileDto>> ListFilesAsync(string userId);
    }
}
