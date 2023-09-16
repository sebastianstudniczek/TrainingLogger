namespace TrainingLogger.Infrastructure.Strava;

// TODO: Register this
public class StravaOptions
{
    public const string Strava = "Strava";

    public int ClientId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string AuthorizationCode { get; set; } = string.Empty;
    public string BaseUri { get; set; } = string.Empty;
    public string GetTokenUri { get; set; } = string.Empty;
    public string GetActivitiesUri { get; set; } = string.Empty;
    public string GetActivityByIdUri { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}