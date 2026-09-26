namespace VehicleWorks.Web.Models;

public class VehicleCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int MinWeightKg { get; set; }

    public int MaxWeightKg { get; set; }

    public string Icon { get; set; } = string.Empty;
}