using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TrainingLogger.Infrastructure.EF;

namespace TrainingLogger.Infrastructure.UnitTests;

internal static class Utils
{
    public static ApplicationDbContext CreateInMemoryContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new ApplicationDbContext(contextOptions);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}
