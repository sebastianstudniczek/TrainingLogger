namespace TrainingLogger.Infrastructure.Strava;

public sealed class StravaOptions
{
    public const string Strava = "Strava";

    public string AuthScheme { get; set; } = default!;
    public int ClientId { get; set; }
    public string ClientSecret { get; set; } = default!;
    public string AuthorizationCode { get; set; } = default!;
    public Uri BaseUri { get; set; } = default!;
    public string TokenExchangePart { get; set; } = default!;
    public string GetActivitiesPart { get; set; } = default!;
    public string GetActivityByIdPart { get; set; } = default!;
    public string WebhookVerifyToken { get; set; } = default!;
}