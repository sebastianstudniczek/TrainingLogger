using Microsoft.AspNetCore.Mvc;
using Serilog;
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

app.MapGet("api/v3/activities/{id}", ([FromRoute] ulong id) =>
{
    var activity = new ActivityFaker()
        .WithId(id)
        .Generate();

    return Results.Ok(activity);
});

app.MapGet("generate-event", () =>
{
    var eventData = new EventDataRequestFaker().Generate();

    return Results.Ok(eventData);
});

app.Run();