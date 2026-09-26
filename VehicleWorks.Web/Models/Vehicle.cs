namespace VehicleWorks.Web.Models;

public class Vehicle
{
    public int Id { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int WeightKg { get; set; }
}