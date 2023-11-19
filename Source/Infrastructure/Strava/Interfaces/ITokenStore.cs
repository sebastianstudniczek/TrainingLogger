using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Interfaces;

internal delegate Task<ApiAccessToken> GetRefreshedToken(string refreshToken);

internal interface ITokenStore
{
    Task<string> GetTokenAsync(CancellationToken token);
}
