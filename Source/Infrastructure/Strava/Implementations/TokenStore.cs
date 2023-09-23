using Microsoft.Extensions.Caching.Memory;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenStore : ITokenStore
{
    private readonly IMemoryCache _cache;

    public TokenStore(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<string> GetTokenAsync(CancellationToken token)
    {
        if (!_cache.TryGetValue("token", out string? jwtToken))
        {

        }

        throw new NotImplementedException();
    }
}
