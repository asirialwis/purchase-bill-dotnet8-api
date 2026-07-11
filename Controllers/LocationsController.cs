using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseBillAPI.Data;

namespace PurchaseBillAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LocationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _context.LocationDetails.ToListAsync();
        return Ok(locations);
    }
}