using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Notifications;
using TrainingLogger.Infrastructure.Notifications.Implementations;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Interfaces;
using TrainingLogger.Shared;

namespace TrainingLogger.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStrava(configuration);

        var connectionString = configuration.GetConnectionString("Sqlite") ?? throw new ArgumentNullException("connectionString cannot be null");
        services.AddDbContext<ApplicationDbContext>((sp, options) => options.UseSqlite(connectionString));

        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();

        return services;
    }

    private static IServiceCollection AddStrava(this IServiceCollection services, IConfiguration configuration)
    {
        var stravaSection = configuration.GetSection(StravaOptions.Strava);
        var stravaOptions = stravaSection.BindOptions<StravaOptions>();
        services.Configure<StravaOptions>(stravaSection);
        
        services.AddMemoryCache();
        services.AddScoped<ITokenStore, TokenStore>();
        services.AddScoped<TokenHandler>();

        services.AddHttpClient(stravaOptions.HttpClientName, (IServiceProvider _, HttpClient client) =>
        {
            client.BaseAddress = new Uri(stravaOptions.BaseUri);
        })
            .AddHttpMessageHandler<TokenHandler>()
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));
}
