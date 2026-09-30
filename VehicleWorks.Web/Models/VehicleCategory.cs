using System.ComponentModel.DataAnnotations;

namespace VehicleWorks.Web.Models;

public class VehicleCategory
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue,
        ErrorMessage = "Minimum weight cannot be negative.")]
    public decimal MinWeightKg { get; set; }

    [Range(0, double.MaxValue,
        ErrorMessage = "Maximum weight cannot be negative.")]
    public decimal? MaxWeightKg { get; set; }

    [Required(ErrorMessage = "Icon is required.")]
    public string Icon { get; set; } = string.Empty;
}