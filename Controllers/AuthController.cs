using Microsoft.AspNetCore.Mvc;
using PurchaseBillAPI.DTOs;
using PurchaseBillAPI.Services;

namespace PurchaseBillAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Utilize constructor injection to pull in the service
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        
        var result = await _authService.AuthenticateAndSyncLocationsAsync(request);

        if (!result.IsSuccess)
        {
            // 401 Unauthorized for bad credentials
            return Unauthorized(new { message = result.ErrorMessage });
        }

        // Set JWT as HttpOnly Cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true if using HTTPS in production
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(60) // Same as JWT expiry
        };
        Response.Cookies.Append("auth_token", result.Token!, cookieOptions);

        // 200 OK for success, also return the token so frontend can decode the payload
        return Ok(new 
        { 
            message = "Login successful", 
            token = result.Token 
        });
    }
}