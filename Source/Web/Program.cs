using Serilog;
using TrainingLogger.Core;
using TrainingLogger.Infrastructure;
using TrainingLogger.Web;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web app");
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    builder.Host.UseSerilog((context, services, config) => config
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services
        .AddCore()
        .AddInfrastructure(config)
        .AddHttpLogging(x => x.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All);

    var app = builder.Build();
    app.UseHttpLogging();

    app.MapGet("/strava-webhook", Endpoints.ValidateSubscription);
    app.MapPost("/strava-webhook", Endpoints.PostEvent);

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}