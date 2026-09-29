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
            .Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Vehicle>()
            .Property(v => v.WeightKg)
            .HasPrecision(10, 2);

        builder.Entity<VehicleCategory>()
                .Property(c => c.MinWeightKg)
                .HasPrecision(10, 2);

            builder.Entity<VehicleCategory>()
                .Property(c => c.MaxWeightKg)
                .HasPrecision(10, 2);
        
        // Seed data for manufacturers, this can be edited to include more manufacturers as needed.
        builder.Entity<Manufacturer>().HasData(
        new Manufacturer { Id = 1, Name = "Mazda" },
        new Manufacturer { Id = 2, Name = "Mercedes" },
        new Manufacturer { Id = 3, Name = "Honda" },
        new Manufacturer { Id = 4, Name = "Ferrari" },
        new Manufacturer { Id = 5, Name = "Toyota" }
    );
    
    // Seed data for vehicle categories, this can be edited to change category limits as needed.
    builder.Entity<VehicleCategory>().HasData(
        new VehicleCategory
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = 500,
            Icon = "🚗"
        },
        new VehicleCategory
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500.01m,
            MaxWeightKg = 2500,
            Icon = "🚙"
        },
        new VehicleCategory
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500.01m,
            MaxWeightKg = null,
            Icon = "🚚"
        }
    );

    }
}