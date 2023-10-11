namespace TrainingLogger.Infrastructure.Strava;

public record StravaOptions
{
    public const string Strava = "Strava";

    public int ClientId { get; set; }
    public required string ClientSecret { get; set; } 
    public required string AuthorizationCode { get; set; }
    public required string BaseUri { get; set; }
    public required string GetTokenUri { get; set; }
    public required string GetActivitiesUri { get; set; }
    public required string GetActivityByIdUri { get; set; }
}