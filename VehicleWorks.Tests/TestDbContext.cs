using Microsoft.EntityFrameworkCore;
using VehicleWorks.Web.Data;

namespace VehicleWorks.Tests;

public static class TestDbContext
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}