using Microsoft.Extensions.Logging;
using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Core.Notifications.ActivityPublished;

internal class ActivityPublishedNotificationHandler(
        IApplicationDbContext dbContext, 
        IActivitiesClient activitiesClient,
        ILogger<ActivityPublishedNotificationHandler> logger) 
    : INotificationHandler<ActivityPublishedNotification> 
{
    public async Task HandleAsync(ActivityPublishedNotification notification, CancellationToken cancellationToken)
    {
        var activity = await activitiesClient.GetActivityByIdAsync(notification.ActivityId, cancellationToken);

        if (activity is null)
        {
            logger.LogError("There is no activity with given id {ActivityId}.", notification.ActivityId);
            return;
        }

        dbContext.Activities.Add(activity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
