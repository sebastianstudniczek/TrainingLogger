namespace TrainingLogger.Infrastructure.Strava;

internal interface ITokenStore
{
    Task<string> GetTokenAsync(CancellationToken token);
}
