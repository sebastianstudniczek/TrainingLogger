namespace TrainingLogger.Infrastructure.Strava.Interfaces;

internal interface ITokenStore
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken);
}
