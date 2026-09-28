using Microsoft.EntityFrameworkCore;
using VehicleWorks.Web.Data;
using VehicleWorks.Web.Models;

namespace VehicleWorks.Web.Services;

public class VehicleService
{
    private readonly AppDbContext _db;

    public VehicleService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _db.Vehicles.Update(vehicle);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);

        if (vehicle == null)
            return;

        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _db.Vehicles
            .Include(v => v.Manufacturer)
            .OrderBy(v => v.OwnerName)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _db.Vehicles
            .Include(v => v.Manufacturer)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}