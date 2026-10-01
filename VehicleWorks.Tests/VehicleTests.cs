using System.ComponentModel.DataAnnotations;
using VehicleWorks.Web.Models;

namespace VehicleWorks.Tests;

public class VehicleTests
{
    [Fact]
    public void Vehicle_WithValidDetails_IsValid()
    {
        var vehicle = new Vehicle
        {
            OwnerName = "John Smith",
            ManufacturerId = 1,
            YearOfManufacture = 2020,
            WeightKg = 1850.75m
        };

        var context = new ValidationContext(vehicle);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            vehicle,
            context,
            results,
            true);

        Assert.True(isValid);
    }

    [Fact]
    public void Vehicle_WithZeroWeight_IsInvalid()
    {
        var vehicle = new Vehicle
        {
            OwnerName = "John Smith",
            ManufacturerId = 1,
            YearOfManufacture = 2020,
            WeightKg = 0
        };

        var context = new ValidationContext(vehicle);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            vehicle,
            context,
            results,
            true);

        Assert.False(isValid);
    }

    [Fact]
    public void Vehicle_WithMoreThanTwoDecimalPlaces_IsInvalid()
    {
        var vehicle = new Vehicle
        {
            OwnerName = "John Smith",
            ManufacturerId = 1,
            YearOfManufacture = 2020,
            WeightKg = 1850.123m
        };

        var context = new ValidationContext(vehicle);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            vehicle,
            context,
            results,
            true);

        Assert.False(isValid);
    }
}