using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.Services;

namespace TrainingLogger.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;
        services.AddSingleton<GetUtcNow>(_ => DateTimeProvider.GetUtcNow);
        services.AddNotificationHandlers();

        return services;
    }


    private static IServiceCollection AddNotificationHandlers(this IServiceCollection services)
    {
        var interfacePredicate = (Type type) => type.GetGenericTypeDefinition() == typeof(INotificationHandler<>);

        var handlers = typeof(Extensions)
            .Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(interfacePredicate) && !x.IsInterface);

        foreach (var handler in handlers)
        {
            var handlerInterface = handler.GetInterfaces().First(interfacePredicate);
            services.AddTransient(handlerInterface, handler);
        }

        return services;
    }
}
