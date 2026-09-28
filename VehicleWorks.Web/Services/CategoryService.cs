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

    public async Task<bool> AddAsync(VehicleCategory category)
    {
        var categories = await GetAllAsync();
        categories.Add(category);

        if (!HasValidCategorySet(categories))
            return false;

        _db.VehicleCategories.Add(category);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task UpdateAsync(VehicleCategory category)
    {
        _db.VehicleCategories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);

        if (category == null)
            return;

        _db.VehicleCategories.Remove(category);
        await _db.SaveChangesAsync();
    }

    public async Task<List<VehicleCategory>> GetAllAsync()
    {
        return await _db.VehicleCategories
            .OrderBy(c => c.MinWeightKg)
            .ToListAsync();
    }

    public async Task<VehicleCategory?> GetByIdAsync(int id)
    {
        return await _db.VehicleCategories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<VehicleCategory?> GetCategoryForWeightAsync(decimal weightKg)
    {
        var categories = await GetAllAsync();

        foreach (var category in categories)
        {
            bool isFirstCategory = category.MinWeightKg == 0;

            bool aboveMinimum = isFirstCategory
                ? weightKg >= category.MinWeightKg
                : weightKg > category.MinWeightKg;

            bool belowMaximum = category.MaxWeightKg == null ||
                                weightKg <= category.MaxWeightKg;

            if (aboveMinimum && belowMaximum)
                return category;
        }

        return null;
    }

    private bool HasValidCategorySet(List<VehicleCategory> categories)
    {
        if (categories.Count == 0)
            return false;

        categories = categories
            .OrderBy(c => c.MinWeightKg)
            .ToList();

        // First category must start at 0
        if (categories[0].MinWeightKg != 0)
            return false;

        for (int i = 0; i < categories.Count - 1; i++)
        {
            var current = categories[i];
            var next = categories[i + 1];

            // Every category except the last must have a maximum
            if (current.MaxWeightKg == null)
                return false;

            // Next category must start exactly 0.01 kg after current
            if (next.MinWeightKg != current.MaxWeightKg + 0.01m)
                return false;
        }

        // Last category must have no maximum
        return categories[^1].MaxWeightKg == null;
    }
}