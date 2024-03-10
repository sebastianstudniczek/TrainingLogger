using Microsoft.AspNetCore.Mvc;
using Serilog;
using TrainingLogger.Infrastructure.Strava.Models;
using TrainingLogger.StravaFakeServer;
using TrainingLogger.StravaFakeServer.Generators;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MapGet("api/v3/activities/{id}", ([FromRoute] long id) =>
{
    var activity = new ActivityFaker()
        .WithId(id)
        .Generate();

    return Results.Ok(activity);
}).RequireAuthorization();

app.MapGet("api/v3/athlete/activities", () => 
{
    ActivityFaker activityFaker = new();

    var activities = activityFaker.GenerateLazy(30).ToList();

    return Results.Ok(activities);
}).RequireAuthorization();

app.MapPost("oauth/token", () =>
{
    var accessToken = new ApiAccessToken()
    {
        AccessToken = Guid.NewGuid().ToString(),
        RefreshToken = Guid.NewGuid().ToString(),
        ExpiresAt = (int)DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds(),
        ExpiresIn = 1222,
        Id = Guid.NewGuid()
    };

    return Results.Ok(accessToken);
});

app.MapGet("generate-event", () =>
{
    var eventData = new EventDataRequestFaker().Generate();

    return Results.Ok(eventData);
});

app.Run();