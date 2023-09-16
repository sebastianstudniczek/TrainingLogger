using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Core;
using TrainingLogger.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<StravaOptions>(
            ctx.Configuration.GetSection(StravaOptions.Strava));

        services
            .AddCore()
            .AddInfrastructure();
    })
    .Build();
