using Microsoft.EntityFrameworkCore;
using VehicleWorks.Web.Data;
using VehicleWorks.Web.Models;

namespace VehicleWorks.Web.Services;

public class CategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<VehicleCategory?> GetCategoryForWeightAsync(decimal weightKg)
    {
        return await _db.VehicleCategories
            .FirstOrDefaultAsync(c =>
                c.MinWeightKg <= weightKg &&
                c.MaxWeightKg >= weightKg);
    }
}