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
        var categories = await _db.VehicleCategories
            .AsNoTracking()
            .OrderBy(c => c.MinWeightKg)
            .ToListAsync();

        // Case 1: new category has no maximum.
        // It becomes the new final category.
        if (!category.MaxWeightKg.HasValue)
        {
            var currentLast = categories.LastOrDefault();

            if (currentLast == null)
                return false;

            // New category must start above the current final category's minimum
            if (category.MinWeightKg <= currentLast.MinWeightKg)
                return false;

            // The current final category now ends where the new one begins
            currentLast.MaxWeightKg = category.MinWeightKg;

            categories.Add(category);

            if (!HasValidCategorySet(categories))
                return false;

            var trackedLast = await _db.VehicleCategories
                .FindAsync(currentLast.Id);

            if (trackedLast == null)
                return false;

            trackedLast.MaxWeightKg = currentLast.MaxWeightKg;

            _db.VehicleCategories.Add(category);

            await _db.SaveChangesAsync();

            return true;
        }

        // Case 2: new category has a maximum.
        // It must be inserted at an existing category boundary.
        var previous = categories
            .FirstOrDefault(c =>
                c.MaxWeightKg.HasValue &&
                c.MaxWeightKg.Value == category.MinWeightKg);

        if (previous == null)
            return false;

        var next = categories
            .FirstOrDefault(c =>
                c.MinWeightKg == category.MinWeightKg);

        if (next == null)
            return false;

        if (category.MaxWeightKg.Value <= category.MinWeightKg)
            return false;

        if (next.MaxWeightKg.HasValue &&
            category.MaxWeightKg.Value > next.MaxWeightKg.Value)
            return false;

        categories.Insert(categories.IndexOf(next), category);

        next.MinWeightKg = category.MaxWeightKg.Value;

        if (!HasValidCategorySet(categories))
            return false;

        var trackedNext = await _db.VehicleCategories
            .FindAsync(next.Id);

        if (trackedNext == null)
            return false;

        trackedNext.MinWeightKg = next.MinWeightKg;

        _db.VehicleCategories.Add(category);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(VehicleCategory category)
    {
        // Get the current database state without tracking
        var categories = await _db.VehicleCategories
            .AsNoTracking()
            .OrderBy(c => c.MinWeightKg)
            .ToListAsync();

        var index = categories.FindIndex(c => c.Id == category.Id);

        if (index == -1)
            return false;

        // Apply the requested change to the untracked copy
        categories[index].Name = category.Name;
        categories[index].MinWeightKg = category.MinWeightKg;
        categories[index].MaxWeightKg = category.MaxWeightKg;
        categories[index].Icon = category.Icon;

        // Adjust neighbouring boundaries
        if (index > 0)
        {
            categories[index - 1].MaxWeightKg =
                categories[index].MinWeightKg;
        }

        if (index < categories.Count - 1)
        {
            if (!categories[index].MaxWeightKg.HasValue)
                return false;

            categories[index + 1].MinWeightKg =
                categories[index].MaxWeightKg.Value;
        }

        // Validate the complete proposed configuration
        if (!HasValidCategorySet(categories))
            return false;

        // Find the existing tracked entities
        foreach (var updatedCategory in categories)
        {
            var trackedCategory = await _db.VehicleCategories
                .FindAsync(updatedCategory.Id);

            if (trackedCategory == null)
                return false;

            trackedCategory.Name = updatedCategory.Name;
            trackedCategory.MinWeightKg = updatedCategory.MinWeightKg;
            trackedCategory.MaxWeightKg = updatedCategory.MaxWeightKg;
            trackedCategory.Icon = updatedCategory.Icon;
        }

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Get current categories without tracking
        var categories = await _db.VehicleCategories
            .AsNoTracking()
            .OrderBy(c => c.MinWeightKg)
            .ToListAsync();

        var index = categories.FindIndex(c => c.Id == id);

        if (index == -1)
            return false;

        // Don't allow deleting the only category
        if (categories.Count == 1)
            return false;

        var categoryToDelete = categories[index];

        // If this isn't the first category,
        // extend the previous category to the deleted category's maximum.
        if (index > 0)
        {
            var previous = categories[index - 1];

            previous.MaxWeightKg = categoryToDelete.MaxWeightKg;

            categories.RemoveAt(index);
        }
        else
        {
            // If deleting the first category,
            // the next category must become the starting category.
            var next = categories[index + 1];

            next.MinWeightKg = 0;

            categories.RemoveAt(index);
        }

        // Make sure the resulting configuration is valid
        if (!HasValidCategorySet(categories))
            return false;

        // Find the tracked entities and apply the validated changes
        foreach (var updatedCategory in categories)
        {
            var trackedCategory = await _db.VehicleCategories
                .FindAsync(updatedCategory.Id);

            if (trackedCategory == null)
                return false;

            trackedCategory.Name = updatedCategory.Name;
            trackedCategory.MinWeightKg = updatedCategory.MinWeightKg;
            trackedCategory.MaxWeightKg = updatedCategory.MaxWeightKg;
            trackedCategory.Icon = updatedCategory.Icon;
        }

        // Delete the selected category
        var trackedDeletedCategory = await _db.VehicleCategories
            .FindAsync(categoryToDelete.Id);

        if (trackedDeletedCategory == null)
            return false;

        _db.VehicleCategories.Remove(trackedDeletedCategory);

        await _db.SaveChangesAsync();

        return true;
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
            .AsNoTracking()
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

    public VehicleCategory? GetCategoryForWeight(decimal weightKg, List<VehicleCategory> categories)
    {
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

    public bool HasValidCategorySet(List<VehicleCategory> categories)
    {
        if (categories.Count == 0)
            return false;

        categories = categories
            .OrderBy(c => c.MinWeightKg)
            .ToList();

        // Categories must start at 0 kg
        if (categories[0].MinWeightKg != 0)
            return false;

        for (int i = 0; i < categories.Count; i++)
        {
            var current = categories[i];

            // Minimum weight cannot be negative
            if (current.MinWeightKg < 0)
                return false;

            // A category with a maximum must have max > min
            if (current.MaxWeightKg.HasValue &&
                current.MaxWeightKg.Value <= current.MinWeightKg)
                return false;

            // The final category must have no maximum
            if (i == categories.Count - 1)
            {
                if (current.MaxWeightKg.HasValue)
                    return false;

                continue;
            }

            var next = categories[i + 1];

            // Every category except the final one needs a maximum
            if (!current.MaxWeightKg.HasValue)
                return false;

            // Next category must begin where the previous one ends
            if (next.MinWeightKg != current.MaxWeightKg.Value)
                return false;
        }

        return true;
    }
}