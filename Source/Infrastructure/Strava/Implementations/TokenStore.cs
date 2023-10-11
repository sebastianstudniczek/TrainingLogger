using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using TrainingLogger.Core.Models;
using TrainingLogger.Core.Services;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenStore : ITokenStore
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly GetUtcNow _getUtcNow;

    public TokenStore(
        ApplicationDbContext dbContext, 
        IMemoryCache cache,
        GetUtcNow getUtcNow)
    {
        _dbContext = dbContext;
        _cache = cache;
        _getUtcNow = getUtcNow;
    }

    public async Task<string> GetTokenAsync(Func<string, Task<ApiAccessToken>> fetchToken, CancellationToken token)
    {
        if (!_cache.TryGetValue(nameof(ApiAccessToken), out ApiAccessToken? cachedToken))
        {
            var newTokenTe = await RefreshTokenAsync(fetchToken, token);
            return newTokenTe.AccessToken;
        }

        DateTimeOffset now = _getUtcNow();
        bool isExpired = cachedToken!.ExpiresAt < now.ToUnixTimeSeconds();

        if (!isExpired)
        {
            return cachedToken.AccessToken;
        }

        var newToken = await RefreshTokenAsync(fetchToken, token);
        await _dbContext.RefreshTokens.ExecuteUpdateAsync(setters =>
            setters
                .SetProperty(x => x.AccessToken, newToken.AccessToken)
                .SetProperty(x => x.AccessToken, newToken.RefreshToken)
                .SetProperty(x => x.ExpiresIn, newToken.ExpiresIn)
                .SetProperty(x => x.ExpiresAt, newToken.ExpiresAt),
            token);

        return newToken.AccessToken;
    }

    private async Task<ApiAccessToken> RefreshTokenAsync(Func<string, Task<ApiAccessToken>> fetchToken, CancellationToken token)
    {
        var accessToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(token)
            ?? throw new StravaAuthTokenNotFound();

        var newToken = await fetchToken(accessToken.RefreshToken);
        _cache.Set(nameof(ApiAccessToken), newToken);

        return newToken;
    }
}
