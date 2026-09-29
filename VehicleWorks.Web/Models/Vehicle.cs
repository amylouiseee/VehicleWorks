using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleWorks.Web.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Owner's name is required.")]
    public string OwnerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a manufacturer.")]
    public int? ManufacturerId { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    [Required(ErrorMessage = "Year of manufacture is required.")]
    [Range(1900, 2100, ErrorMessage = "Please enter a valid year.")]
    public int? YearOfManufacture { get; set; }

    [Required(ErrorMessage = "Weight is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
    public decimal? WeightKg { get; set; }

    [NotMapped]
    public VehicleCategory? Category { get; set; }
}