using Microsoft.Extensions.Options;
using TrainingLogger.Shared;

namespace TrainingLogger.StravaFakeServer;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        var options = config
            .GetSection(TrainingLoggerOptions.TrainingLogger)
            .BindOptions<TrainingLoggerOptions>();

        services
            .AddOptions<TrainingLoggerOptions>()
            .Bind(config.GetSection(TrainingLoggerOptions.TrainingLogger));

        services.AddHttpClient<ActivityPublisher>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<TrainingLoggerOptions>>();
            client.BaseAddress = new Uri(options.Value.BaseUri);
        });

        if (options.PublishActivities)
        {
            services.AddHostedService<ActivityPublisher>();
        }

        return services;
    }
}