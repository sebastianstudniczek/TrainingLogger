using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Core.Contracts;

namespace TrainingLogger.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
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
