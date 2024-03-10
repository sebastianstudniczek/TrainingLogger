using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.EF;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<ApiAccessToken> ApiAccessTokens { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
