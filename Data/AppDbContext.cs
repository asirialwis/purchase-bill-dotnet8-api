using Microsoft.EntityFrameworkCore;
namespace PurchaseBillAPI.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Models.LocationDetail> LocationDetails { get; set; }
}