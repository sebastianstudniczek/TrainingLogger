using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Interfaces;

namespace TrainingLogger.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StravaOptions>(configuration.GetSection(StravaOptions.Strava));
        services.Configure<StravaWebhookOptions>(configuration.GetSection(StravaWebhookOptions.StravaWebhook));

        services.AddMemoryCache();
        services.AddScoped<ITokenStore, TokenStore>();
        services.AddHttpClient<ITokenContext, TokenContext>((IServiceProvider sp, HttpClient client) =>
        {
            var stravaOptions = sp.GetRequiredService<StravaOptions>();
            client.BaseAddress = new Uri(stravaOptions.BaseUri);
        });

        var connectionString = configuration.GetConnectionString("Sqlite");

        if (connectionString is null)
        {
            throw new ArgumentNullException("connectionString cannot be null");
        }

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}
