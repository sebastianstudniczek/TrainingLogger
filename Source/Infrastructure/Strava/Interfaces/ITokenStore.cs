using TrainingLogger.Core.Models;

namespace TrainingLogger.Infrastructure.Strava.Interfaces;

internal delegate Task<ApiAccessToken> GetRefreshedToken(string refreshToken);

internal interface ITokenStore
{
    Task<string> GetTokenAsync(GetRefreshedToken fetchToken, CancellationToken token);
}
