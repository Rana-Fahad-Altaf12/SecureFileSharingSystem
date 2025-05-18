using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecureFileSharingSystem.Application.Interfaces;
using SecureFileSharingSystem.Domain.DTOs;
using SecureFileSharingSystem.Infrastructure.Encryption;
using SecureFileSharingSystem.Persistence;
using SecureFileSharingSystem.Persistence.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SecureFileSharingSystem.Infrastructure.FileManagement
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _rootPath;
        private readonly string _encryptionKey;

        public FileService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "EncryptedFiles");
            _encryptionKey = configuration["Encryption:AesKey"];

            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> UploadAsync(IFormFile file, string userId)
        {
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var encryptedData = AesEncryptionService.Encrypt(ms.ToArray(), _encryptionKey);

                var storedFileName = $"{Guid.NewGuid()}.enc";
                var fullPath = Path.Combine(_rootPath, storedFileName);
                await File.WriteAllBytesAsync(fullPath, encryptedData);

                var record = new FileRecord
                {
                    FileName = file.FileName,
                    StoredFileName = storedFileName,
                    UploadedByUserId = userId
                };

                _context.FileRecords.Add(record);
                await _context.SaveChangesAsync();

                return record.Id.ToString();
            }
            catch (Exception ex)
            {
                // Optional: Log error here
                throw new Exception("Failed to upload file: " + ex.Message);
            }
        }

        public async Task<FileDownloadResult> DownloadAsync(string fileId, string userId)
        {
            var record = await _context.FileRecords
                .FirstOrDefaultAsync(x => x.Id.ToString() == fileId && x.UploadedByUserId == userId);

            if (record == null)
                throw new Exception("File not found or access denied");

            var encryptedPath = Path.Combine(_rootPath, record.StoredFileName);
            if (!File.Exists(encryptedPath))
                throw new Exception("Encrypted file not found on disk");

            var encryptedData = await File.ReadAllBytesAsync(encryptedPath);
            var decryptedData = AesEncryptionService.Decrypt(encryptedData, _encryptionKey);

            return new FileDownloadResult
            {
                Data = decryptedData,
                FileName = record.FileName // includes the original extension like ".pdf"
            };
        }


        public async Task<List<FileDto>> ListFilesAsync(string userId)
        {
            return await _context.FileRecords
                .Where(fr => fr.UploadedByUserId == userId)
                .Select(fr => new FileDto
                {
                    Id = fr.Id.ToString(),
                    FileName = fr.FileName,
                    UploadedAt = fr.UploadedAt
                })
                .ToListAsync();
        }
    }
}
