using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.EF;

internal sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ApiAccessToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
