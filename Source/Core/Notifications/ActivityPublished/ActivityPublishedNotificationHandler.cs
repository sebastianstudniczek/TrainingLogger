using MediatR;
using Microsoft.Extensions.Logging;
using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Core.Notifications.ActivityPublished;

internal class ActivityPublishedNotificationHandler : INotificationHandler<ActivityPublishedNotification> {
    private readonly IApplicationDbContext _dbContext;
    private readonly IActivitiesClient _activitiesClient;
    private readonly ILogger<ActivityPublishedNotificationHandler> _logger;

    public ActivityPublishedNotificationHandler(IApplicationDbContext dbContext, IActivitiesClient activitiesClient, ILogger<ActivityPublishedNotificationHandler> logger) {
        _dbContext = dbContext;
        _activitiesClient = activitiesClient;
        _logger = logger;
    }

    public async Task Handle(ActivityPublishedNotification notification, CancellationToken cancellationToken) {
        var activity = await _activitiesClient.GetActivityByIdAsync(notification.ActivityId, cancellationToken);

        if (activity is null) {
            _logger.LogError("There is no activity with given id {ActivityId}.", notification.ActivityId);
            return;
        }

        _dbContext.Activities.Add(activity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
