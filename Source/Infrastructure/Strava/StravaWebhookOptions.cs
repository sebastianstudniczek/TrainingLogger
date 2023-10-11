namespace TrainingLogger.Infrastructure.Strava;

public record StravaWebhookOptions
{
    public const string StravaWebhook = "StravaWebhook";

    public required string VerifyToken { get; init; }
}
