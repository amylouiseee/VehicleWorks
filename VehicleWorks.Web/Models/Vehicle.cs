namespace VehicleWorks.Web.Models;

using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    public int Id { get; set; }

    [Required]
    public string OwnerName { get; set; } = string.Empty;

    [Required]
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    [Required]
    public int YearOfManufacture { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal WeightKg { get; set; }
}