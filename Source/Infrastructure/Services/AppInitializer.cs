using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Infrastructure.EF;

namespace TrainingLogger.Infrastructure.Services;

internal sealed class AppInitializer(IServiceProvider serviceProvider, ILogger<AppInitializer> logger) : IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<AppInitializer> _logger = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _logger.LogInformation("Applying db migrations...");
        await dbContext.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Checking activities table...");
        var newestActivity = dbContext
            .Activities
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefault();

        var from = newestActivity?.StartDate;

        if (from is null)
        {
            _logger.LogInformation("No activities saved. Seeding database...");
        }
        else
        {
            _logger.LogInformation("Downloading activities since {FromDate}.", from.Value);
        }

        var activitiesClient = scope.ServiceProvider.GetRequiredService<IActivitiesClient>();
        var activities = (await activitiesClient
            .GetActivitiesAsync(from, cancellationToken))
            .Select(x => x.AsEntity())
            .ToList();

        _logger.LogInformation("Found {Count} new activities. Saving...", activities.Count);

        await dbContext.Activities.AddRangeAsync(activities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}