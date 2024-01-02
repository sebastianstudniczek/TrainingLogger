using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Interfaces;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenStore(
    ApplicationDbContext dbContext,
    IMemoryCache cache,
    TimeProvider timeProvider,
    GetRefreshedToken getRefreshToken) : ITokenStore
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMemoryCache _cache = cache;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly GetRefreshedToken _getRefreshToken = getRefreshToken;

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        long unixTimeSeconds = _timeProvider.GetUtcNow().ToUnixTimeSeconds();
        var tokenFactory = GetTokenFactory(_getRefreshToken, unixTimeSeconds, cancellationToken);
        var token = await _cache.GetOrCreateAsync(nameof(ApiAccessToken), tokenFactory);

        return token is null
            ? throw new StravaAuthTokenNotFound()
            : token.AccessToken;
    }

    private Func<ICacheEntry, Task<ApiAccessToken?>> GetTokenFactory(GetRefreshedToken fetchNewToken, long unixTimeSeconds, CancellationToken cancellationToken) =>
        async (cacheEntry) =>
        {
            var savedToken = await _dbContext
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
            await _dbContext
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
