using Microsoft.Extensions.Logging;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;

namespace TrainingLogger.Core.Notifications.ActivityPublished;

internal class ActivityPublishedNotificationHandler(
    IApplicationDbContext dbContext,
    IActivitiesClient activitiesClient,
    ILogger<ActivityPublishedNotificationHandler> logger) : INotificationHandler<ActivityPublishedNotification> 
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IActivitiesClient _activitiesClient = activitiesClient;
    private readonly ILogger<ActivityPublishedNotificationHandler> _logger = logger;

    public async Task HandleAsync(ActivityPublishedNotification notification, CancellationToken cancellationToken)
    {
        var activityDto = await _activitiesClient.GetActivityByIdAsync(notification.ActivityId, cancellationToken);

        if (activityDto is null)
        {
            _logger.LogError("There is no activity with given id {ActivityId}.", notification.ActivityId);
            return;
        }

        _dbContext.Activities.Add(activityDto.AsEntity());
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
