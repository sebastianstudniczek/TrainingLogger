namespace TrainingLogger.StravaFakeServer;

public class TrainingLoggerOptions
{
    public const string TrainingLogger = "TrainingLogger";

    public string BaseUri { get; set; } = default!;
    public string SubscriptionEndpoint { get; set; } = default!;
    public bool PublishActivities { get; set; }
}