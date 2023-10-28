using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Core.Services;

namespace TrainingLogger.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddSingleton<GetUtcNow>(_ => DateTimeProvider.GetUtcNow);

        return services;
    }
}
