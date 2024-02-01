using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Core.Notifications.ActivityPublished;

public record ActivityPublishedNotification(long ActivityId) : INotification;
