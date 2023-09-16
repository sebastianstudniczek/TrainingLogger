using Microsoft.Extensions.DependencyInjection;
using StravaActivityExtractor.Core.Services;
using StravaActivityExtractor.Core.Services.Implementations;

namespace StravaActivityExtractor.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IStravaClient, StravaClient>();
        services.AddScoped<ITokenStore, TokenStore>();

        return services;
    }
}
