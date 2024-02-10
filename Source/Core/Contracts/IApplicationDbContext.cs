using Microsoft.EntityFrameworkCore;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Core.Contracts;

public interface IApplicationDbContext
{
    public DbSet<Activity> Activities { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken token);
}
