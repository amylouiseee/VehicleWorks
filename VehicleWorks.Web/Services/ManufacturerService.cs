using Microsoft.EntityFrameworkCore;
using VehicleWorks.Web.Data;
using VehicleWorks.Web.Models;

namespace VehicleWorks.Web.Services;

public class ManufacturerService
{
    private readonly AppDbContext _db;

    public ManufacturerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Manufacturer>> GetAllAsync()
    {
        return await _db.Manufacturers
            .OrderBy(m => m.Name)
            .ToListAsync();
    }
}