using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureFileSharingSystem.Application.Interfaces;
using SecureFileSharingSystem.Persistence.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace SecureFileSharingSystem.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApiKeyValidator _apiKeyValidator;
        private readonly IAuthService _authService;


        public FileController(IFileService fileService, UserManager<ApplicationUser> userManager, IApiKeyValidator apiKeyValidator, IAuthService authService)
        {
            _fileService = fileService;
            _userManager = userManager;
            _apiKeyValidator = apiKeyValidator;
            _authService = authService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return Unauthorized("User not found");

                var fileId = await _fileService.UploadAsync(file, user.Id);
                return Ok(new { fileId });
            }
            catch (Exception ex)
            {
                // Optionally log the error
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var email = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized("User not found");

            var decryptedData = await _fileService.DownloadAsync(id, user.Id);

            return File(decryptedData.Data, "application/octet-stream", "decrypted_file");
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListFiles()
        {
            var email = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized("User not found");

            var files = await _fileService.ListFilesAsync(user.Id);
            return Ok(files);
        }

        [AllowAnonymous]
        [HttpGet("thirdparty/download/{id}")]
        public async Task<IActionResult> DownloadForThirdParty(string id, [FromQuery] string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return Unauthorized("API Key is required.");

            // Step 1: Lookup user by API Key
            var user = await _authService.GetUserByApiKeyAsync(apiKey);
            if (user == null)
                return Unauthorized("Invalid API Key.");

            // Step 2: Validate file ownership
            var fileData = await _fileService.DownloadAsync(id, user.Id);
            if (fileData == null)
                return NotFound("File not found or access denied.");

            var fileResult = new FileContentResult(fileData.Data, "application/octet-stream");
            fileResult.FileDownloadName = fileData.FileName;

            // manually add header to control filename formatting
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileData.FileName}\"");
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            return fileResult;
        }

    }
}
