using PurchaseBillAPI.DTOs;

namespace PurchaseBillAPI.Services;

public interface IAuthService
{
    Task<(bool IsSuccess, string ErrorMessage, string? Token)> AuthenticateAndSyncLocationsAsync(LoginDto request);
}