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

    public async Task AddAsync(VehicleCategory category)
    {
        _db.VehicleCategories.Add(category);
        await _db.SaveChangesAsync();
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

    public async Task<bool> HasValidRangeAsync(VehicleCategory category)
    {
        if (category.MinWeightKg < 0)
            return false;

        if (category.MaxWeightKg.HasValue &&
            category.MaxWeightKg <= category.MinWeightKg)
            return false;

        var categories = await GetAllAsync();

        categories = categories
            .Where(c => c.Id != category.Id)
            .OrderBy(c => c.MinWeightKg)
            .ToList();

        foreach (var existing in categories)
        {
            var overlaps =
                category.MinWeightKg <= existing.MaxWeightKg &&
                (category.MaxWeightKg == null ||
                existing.MinWeightKg <= category.MaxWeightKg);

            if (overlaps)
                return false;
        }

        return true;
    }

    public async Task<bool> HasValidCategoriesAsync()
    {
        var categories = await GetAllAsync();

        if (categories.Count == 0)
            return false;

        if (categories[0].MinWeightKg != 0)
            return false;

        for (int i = 0; i < categories.Count - 1; i++)
        {
            var current = categories[i];
            var next = categories[i + 1];

            if (current.MaxWeightKg == null)
                return false;

            if (next.MinWeightKg != current.MaxWeightKg + 0.01m)
                return false;
        }

        return categories[^1].MaxWeightKg == null;
    }
}