using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TrainingLogger.Core.Services;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Interfaces;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenStore(
        ApplicationDbContext dbContext,
        IMemoryCache cache,
        GetUtcNow getUtcNow,
        GetRefreshedToken getRefreshToken)
    : ITokenStore
{
    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        long unixTimeSeconds = getUtcNow().ToUnixTimeSeconds();
        var tokenFactory = GetTokenFactory(getRefreshToken, unixTimeSeconds, cancellationToken);
        var token = await cache.GetOrCreateAsync(nameof(ApiAccessToken), tokenFactory);

        return token is null
            ? throw new StravaAuthTokenNotFound()
            : token.AccessToken;
    }

    private Func<ICacheEntry, Task<ApiAccessToken?>> GetTokenFactory(GetRefreshedToken fetchNewToken, long unixTimeSeconds, CancellationToken cancellationToken) =>
        async (cacheEntry) =>
        {
            var savedToken = await dbContext
                .RefreshTokens
                .FirstOrDefaultAsync(cancellationToken);

            if (savedToken is null)
            {
                cacheEntry.Dispose();
                return null;
            }

            if (savedToken.ExpiresAt > unixTimeSeconds)
            {
                return savedToken;
            }

            var refreshedToken = await fetchNewToken(savedToken.RefreshToken);
            await dbContext
                .RefreshTokens
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.AccessToken, refreshedToken.AccessToken)
                    .SetProperty(x => x.RefreshToken, refreshedToken.RefreshToken)
                    .SetProperty(x => x.ExpiresIn, refreshedToken.ExpiresIn)
                    .SetProperty(x => x.ExpiresAt, refreshedToken.ExpiresAt),
                    cancellationToken);

            return refreshedToken;
        };
}
