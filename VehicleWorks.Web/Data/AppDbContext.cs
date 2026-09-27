using Microsoft.EntityFrameworkCore;
using VehicleWorks.Web.Models;

namespace VehicleWorks.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
}