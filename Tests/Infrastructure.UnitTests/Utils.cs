using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TrainingLogger.Infrastructure.EF;

namespace TrainingLogger.Infrastructure.UnitTests;

internal static class Utils
{
    public const string SqliteInMemoryConnectionString = "Filename=:memory:";

    public static ApplicationDbContext CreateInMemoryContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .LogTo(Console.WriteLine)
            .Options;

        var dbContext = new ApplicationDbContext(contextOptions);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}