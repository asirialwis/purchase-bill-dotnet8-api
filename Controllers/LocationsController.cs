using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseBillAPI.Services;

namespace PurchaseBillAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _locationService.GetAllLocationsAsync();

        if (!locations.Any())
        {
            return NotFound(new { message = "No locations found." });
        }

        return Ok(locations);
    }
}