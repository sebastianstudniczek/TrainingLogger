using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Infrastructure.Notifications;

public interface INotificationDispatcher
{
    public Task PublishAsync<T>(T notification, CancellationToken token) where T : INotification;
}
