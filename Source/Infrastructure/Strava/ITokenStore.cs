using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava;

internal interface ITokenStore
{
    Task<string> GetTokenAsync(Func<string, Task<ApiAccessToken>> fetchToken, CancellationToken token);
}
