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
        // [ApiController] automatically handles ModelState validation (the DTO Data Annotations)
        // and returns a 400 Bad Request if it fails, so we don't need manual checking here.

        var result = await _authService.AuthenticateAndSyncLocationsAsync(request);

        if (!result.IsSuccess)
        {
            // 401 Unauthorized for bad credentials
            return Unauthorized(new { message = result.ErrorMessage });
        }

        // 200 OK for success
        return Ok(new 
        { 
            message = "Login successful", 
            token = result.Token 
        });
    }
}