using VehicleWorks.Web.Models;
using VehicleWorks.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace VehicleWorks.Tests;

public class CategoryServiceTests
{
    [Fact]
    public async Task AddCategory_AddsCategorySuccessfully()
    {
        using var db = TestDbContext.Create();

        var service = new CategoryService(db);

        var existingCategory = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = null,
            Icon = "🚗"
        };

        db.VehicleCategories.Add(existingCategory);
        await db.SaveChangesAsync();

        var newCategory = new VehicleCategory
        {
            Name = "Heavy",
            MinWeightKg = 1000,
            MaxWeightKg = null,
            Icon = "🚚"
        };

        var result = await service.AddAsync(newCategory);

        Assert.True(result);
        Assert.Equal(2, await db.VehicleCategories.CountAsync());
    }

    [Fact]
    public async Task AddCategory_UpdatesExistingCategoryRange()
    {
        using var db = TestDbContext.Create();

        var light = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = 500,
            Icon = "🚗"
        };

        var heavy = new VehicleCategory
        {
            Name = "Heavy",
            MinWeightKg = 500,
            MaxWeightKg = null,
            Icon = "🚚"
        };

        db.VehicleCategories.AddRange(light, heavy);
        await db.SaveChangesAsync();

        var service = new CategoryService(db);

        var medium = new VehicleCategory
        {
            Name = "Medium",
            MinWeightKg = 500,
            MaxWeightKg = 1000,
            Icon = "🚙"
        };

        var result = await service.AddAsync(medium);

        Assert.True(result);

        var updatedHeavy = await db.VehicleCategories
            .FirstAsync(c => c.Name == "Heavy");

        Assert.Equal(1000, updatedHeavy.MinWeightKg);
    }

    [Fact]
    public async Task UpdateCategory_UpdatesNeighbouringCategory()
    {
        using var db = TestDbContext.Create();

        var light = new VehicleCategory
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = 500,
            Icon = "🚗"
        };

        var medium = new VehicleCategory
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500,
            MaxWeightKg = 2500,
            Icon = "🚙"
        };

        var heavy = new VehicleCategory
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500,
            MaxWeightKg = null,
            Icon = "🚚"
        };

        db.VehicleCategories.AddRange(light, medium, heavy);
        await db.SaveChangesAsync();

        var service = new CategoryService(db);

        medium.MaxWeightKg = 2000;

        var result = await service.UpdateAsync(medium);

        Assert.True(result);

        var updatedHeavy = await db.VehicleCategories
            .FirstAsync(c => c.Id == 3);

        Assert.Equal(2000, updatedHeavy.MinWeightKg);
    }

    [Fact]
    public async Task DeleteCategory_ExtendsPreviousCategory()
    {
        using var db = TestDbContext.Create();

        var light = new VehicleCategory
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = 500,
            Icon = "🚗"
        };

        var medium = new VehicleCategory
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500,
            MaxWeightKg = 2500,
            Icon = "🚙"
        };

        var heavy = new VehicleCategory
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500,
            MaxWeightKg = null,
            Icon = "🚚"
        };

        db.VehicleCategories.AddRange(light, medium, heavy);
        await db.SaveChangesAsync();

        var service = new CategoryService(db);

        var result = await service.DeleteAsync(2);

        Assert.True(result);

        var updatedLight = await db.VehicleCategories
            .FirstAsync(c => c.Id == 1);

        Assert.Equal(2500, updatedLight.MaxWeightKg);

        Assert.Null(
            await db.VehicleCategories
                .FirstOrDefaultAsync(c => c.Id == 2));
    }
}