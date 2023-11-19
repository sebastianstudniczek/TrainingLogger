namespace TrainingLogger.Infrastructure.Strava;

public sealed class StravaOptions
{
    public const string Strava = "Strava";

    public string AuthScheme { get; set; } = default!;
    public string HttpClientName { get; set; } = default!;
    public int ClientId { get; set; }
    public string ClientSecret { get; set; } = default!;
    public string AuthorizationCode { get; set; } = default!;
    public string BaseUri { get; set; } = default!;
    public string GetTokenUri { get; set; } = default!;
    public string GetActivitiesUri { get; set; } = default!;
    public string GetActivityByIdUri { get; set; } = default!;
    public string WebhookVerifyToken { get; set; } = default!;
}