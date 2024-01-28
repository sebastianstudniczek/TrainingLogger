using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using TrainingLogger.Infrastructure.EF;
using TrainingLogger.Infrastructure.Strava.Exceptions;
using TrainingLogger.Infrastructure.Strava.Interfaces;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenStore(
    ApplicationDbContext dbContext,
    IMemoryCache cache,
    TimeProvider timeProvider,
    IHttpClientFactory httpClientFactory,
    IOptions<StravaOptions> stravaOptions,
    IOptions<ClientCredentials> clientCredentials) : ITokenStore
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMemoryCache _cache = cache;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly StravaOptions _stravaOptions = stravaOptions.Value;
    private readonly ClientCredentials _clientCredentials = clientCredentials.Value;

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        long unixTimeSeconds = _timeProvider.GetUtcNow().ToUnixTimeSeconds();
        var tokenFactory = GetTokenFactory(unixTimeSeconds, cancellationToken);
        var token = await _cache.GetOrCreateAsync(nameof(ApiAccessToken), tokenFactory);

        return token is null
            ? throw new StravaAuthTokenNotFound()
            : token.AccessToken;
    }

    private Func<ICacheEntry, Task<ApiAccessToken?>> GetTokenFactory(long unixTimeSeconds, CancellationToken cancellationToken) =>
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

            var refreshedToken = await GetRefreshedToken(savedToken.RefreshToken, cancellationToken);

            if (refreshedToken is null)
            {
                return null;
            }

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


    private async Task<ApiAccessToken?> GetRefreshedToken(string refreshToken, CancellationToken token)
    {
        var stravaClient = _httpClientFactory.CreateClient(Consts.StravaClientName);
        var request = new TokenExchangeRequest(_clientCredentials.Id, _clientCredentials.Secret, refreshToken);
        var serializerOptios = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        var response = await stravaClient.PostAsJsonAsync(_stravaOptions.TokenExchangePart, request, serializerOptios, token);
        var accessToken = await response.Content.ReadFromJsonAsync<ApiAccessToken>(token);

        return accessToken;
    }
}
