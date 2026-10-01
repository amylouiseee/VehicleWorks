using VehicleWorks.Web.Models;
using VehicleWorks.Web.Services;

namespace VehicleWorks.Tests;

public class CategoryTests
{
    private readonly CategoryService service = new(null!);

    private readonly List<VehicleCategory> categories =
    [
        new()
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0,
            MaxWeightKg = 500
        },
        new()
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500,
            MaxWeightKg = 2500
        },
        new()
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500,
            MaxWeightKg = null
        }
    ];

    [Fact]
    public void Weight_500kg_BelongsToLightCategory()
    {
        var result = service.GetCategoryForWeight(500, categories);

        Assert.Equal("Light", result?.Name);
    }

    [Fact]
    public void Weight_500Point01kg_BelongsToMediumCategory()
    {
        var result = service.GetCategoryForWeight(500.01m, categories);

        Assert.Equal("Medium", result?.Name);
    }

    [Fact]
    public void Weight_2500kg_BelongsToMediumCategory()
    {
        var result = service.GetCategoryForWeight(2500, categories);

        Assert.Equal("Medium", result?.Name);
    }

    [Fact]
    public void Weight_2500Point01kg_BelongsToHeavyCategory()
    {
        var result = service.GetCategoryForWeight(2500.01m, categories);

        Assert.Equal("Heavy", result?.Name);
    }

    [Fact]
    public void Categories_WithGap_AreInvalid()
    {
        var invalidCategories = new List<VehicleCategory>
        {
            new()
            {
                MinWeightKg = 0,
                MaxWeightKg = 500
            },
            new()
            {
                MinWeightKg = 600,
                MaxWeightKg = null
            }
        };

        Assert.False(service.HasValidCategorySet(invalidCategories));
    }

    [Fact]
    public void Categories_WithOverlap_AreInvalid()
    {
        var invalidCategories = new List<VehicleCategory>
        {
            new()
            {
                MinWeightKg = 0,
                MaxWeightKg = 600
            },
            new()
            {
                MinWeightKg = 500,
                MaxWeightKg = null
            }
        };

        Assert.False(service.HasValidCategorySet(invalidCategories));
    }

    [Fact]
    public void Categories_NotStartingAtZero_AreInvalid()
    {
        var invalidCategories = new List<VehicleCategory>
        {
            new()
            {
                MinWeightKg = 100,
                MaxWeightKg = null
            }
        };

        Assert.False(service.HasValidCategorySet(invalidCategories));
    }

    [Fact]
    public void FinalCategory_WithMaximum_IsInvalid()
    {
        var invalidCategories = new List<VehicleCategory>
        {
            new()
            {
                MinWeightKg = 0,
                MaxWeightKg = 500
            },
            new()
            {
                MinWeightKg = 500,
                MaxWeightKg = 1000
            }
        };

        Assert.False(service.HasValidCategorySet(invalidCategories));
    }

    [Fact]
    public void Category_WithMaximumLessThanMinimum_IsInvalid()
    {
        var invalidCategories = new List<VehicleCategory>
        {
            new()
            {
                MinWeightKg = 0,
                MaxWeightKg = 500
            },
            new()
            {
                MinWeightKg = 600,
                MaxWeightKg = 550
            },
            new()
            {
                MinWeightKg = 550,
                MaxWeightKg = null
            }
        };

        Assert.False(service.HasValidCategorySet(invalidCategories));
    }
}