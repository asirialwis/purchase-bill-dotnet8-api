using Microsoft.EntityFrameworkCore;
using PurchaseBillAPI.Data;
using PurchaseBillAPI.Models;

namespace PurchaseBillAPI.Services;

public class LocationService : ILocationService
{
    private readonly AppDbContext _context;
    private readonly ILogger<LocationService> _logger;

    public LocationService(AppDbContext context, ILogger<LocationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<LocationDetail>> GetAllLocationsAsync()
    {
        try
        {
            // AsNoTracking is a massive performance boost for read-only queries
            return await _context.LocationDetails
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching locations from the database.");
            // Return an empty list or throw a custom exception 
            return Enumerable.Empty<LocationDetail>(); 
        }
    }
}