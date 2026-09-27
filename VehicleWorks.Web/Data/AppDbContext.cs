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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Vehicle>()
            .Property(v => v.WeightKg)
            .HasPrecision(10, 2);
        
        // Seed data for manufacturers, this can be edited to include more manufacturers as needed.
        builder.Entity<Manufacturer>().HasData(
        new Manufacturer { Id = 1, Name = "Mazda" },
        new Manufacturer { Id = 2, Name = "Mercedes" },
        new Manufacturer { Id = 3, Name = "Honda" },
        new Manufacturer { Id = 4, Name = "Ferrari" },
        new Manufacturer { Id = 5, Name = "Toyota" }
    );
    }
}