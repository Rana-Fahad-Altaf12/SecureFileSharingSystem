using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFileSharingSystem.Application.Interfaces;

namespace SecureFileSharingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        try
        {
            var token = await _authService.RegisterAsync(request.Email, request.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred", detail = ex.Message });
        }
    }
}

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}