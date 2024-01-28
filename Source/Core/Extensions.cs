using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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

        var handlers = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && interfacePredicate(y)) && !x.IsInterface)
            .Select(x => new
            {
                Type = x,
                Interface = x.GetInterfaces().First(interfacePredicate)
            });

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.Interface, handler.Type);
        }

        return services;
    }
}
