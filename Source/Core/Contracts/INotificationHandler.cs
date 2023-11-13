namespace TrainingLogger.Core.Contracts;

// Marker interface
public interface INotification { }

public interface INotificationHandler<T> where T : INotification
{
    public Task HandleAsync(T notification, CancellationToken token);
}
