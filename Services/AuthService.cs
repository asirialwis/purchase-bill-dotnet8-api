using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PurchaseBillAPI.Data;
using PurchaseBillAPI.DTOs;
using PurchaseBillAPI.Models;

namespace PurchaseBillAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IHttpClientFactory httpClientFactory, ILogger<AuthService> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, string ErrorMessage, string? Token)> AuthenticateAndSyncLocationsAsync(LoginDto request)
    {
        try
        {
            var payload = new
            {
                API_Action = "GetLoginData",
                Device_Id = "D001",
                Sync_Time = "",
                Company_Code = request.Username,
                API_Body = new { Username = request.Username, Pw = request.Password }
            };

            var client = _httpClientFactory.CreateClient();
            var targetUrl = "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };

            var response = await client.PostAsJsonAsync(targetUrl, payload, jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("External API returned {StatusCode} during login attempt for {User}", response.StatusCode, request.Username);
                return (false, "External authentication service is currently unavailable.", null);
            }

            var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();

            // Validate successful JSON structure
            if (!responseData.TryGetProperty("Response_Body", out var responseBody) || 
                responseBody.ValueKind != JsonValueKind.Array || 
                responseBody.GetArrayLength() == 0)
            {
                _logger.LogWarning("Invalid credentials or unexpected JSON structure for user: {User}", request.Username);
                return (false, "Invalid credentials.", null);
            }

            var firstUserObject = responseBody[0];

            // Check for specific error message inside the payload (Doc_Msg)
            if (firstUserObject.TryGetProperty("Doc_Msg", out var docMsg))
            {
                return (false, docMsg.GetString() ?? "Invalid Login Details.", null);
            }

            // Extract and sync locations
            if (firstUserObject.TryGetProperty("User_Locations", out var locationsElement) && locationsElement.ValueKind == JsonValueKind.Array)
            {
                var locations = locationsElement.Deserialize<List<LocationDetail>>();
                
                if (locations != null && locations.Any())
                {
                    await SyncLocationsToDatabaseAsync(locations);
                }
            }
            else
            {
                return (false, "Authentication succeeded but location data was missing.", null);
            }

            // Hardcoded token
            return (true, string.Empty, "enhanzer-auth-token");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during authentication for user {User}", request.Username);
            return (false, "An internal server error occurred while processing your request.", null);
        }
    }

    private async Task SyncLocationsToDatabaseAsync(List<LocationDetail> locations)
    {
        // Fetch existing location codes to prevent redundant DB calls
        var existingCodes = await _context.LocationDetails
                                          .Select(l => l.Location_Code)
                                          .ToListAsync();

        var newLocations = locations.Where(l => !existingCodes.Contains(l.Location_Code)).ToList();

        if (newLocations.Any())
        {
            await _context.LocationDetails.AddRangeAsync(newLocations);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully synced {Count} new locations.", newLocations.Count);
        }
    }
}