using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Infrastructure.Notifications.Implementations;

internal class NotificationDispatcher : INotificationDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<T>(T notification, CancellationToken token) where T : INotification
    {
        var handlers = _serviceProvider
            .GetRequiredService<IEnumerable<INotificationHandler<T>>>();
        // TODO: Think about maybe wiring up Channel and background processing if there will be problem
        // Cause this approach relies on scoped resourced to the request
        var tasks = handlers.Select(x => x.HandleAsync(notification, token));

        await Task.WhenAll(tasks);
    }
}
