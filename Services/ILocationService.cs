using PurchaseBillAPI.Models;

namespace PurchaseBillAPI.Services;

public interface ILocationService
{
    Task<IEnumerable<LocationDetail>> GetAllLocationsAsync();
}