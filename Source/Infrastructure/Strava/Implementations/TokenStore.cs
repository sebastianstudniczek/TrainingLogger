using Flurl;
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
                .ApiAccessTokens
                .FirstOrDefaultAsync(cancellationToken);

            if (savedToken?.ExpiresAt > unixTimeSeconds)
            {
                return savedToken;
            }

            var refreshedToken = await GetRefreshedToken(_clientCredentials.RefreshToken, cancellationToken);
            
            if (refreshedToken is null)
            {
                cacheEntry.Dispose();
                return null;
            }

            if (savedToken is null)
            {
                await _dbContext.ApiAccessTokens.AddAsync(refreshedToken, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            } 
            else
            {
                await _dbContext
                    .ApiAccessTokens
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.AccessToken, refreshedToken.AccessToken)
                        .SetProperty(x => x.RefreshToken, refreshedToken.RefreshToken)
                        .SetProperty(x => x.ExpiresIn, refreshedToken.ExpiresIn)
                        .SetProperty(x => x.ExpiresAt, refreshedToken.ExpiresAt),
                        cancellationToken);
            }

            return refreshedToken;
        };

    private async Task<ApiAccessToken?> GetRefreshedToken(string refreshToken, CancellationToken token)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var request = new TokenExchangeRequest(_clientCredentials.Id, _clientCredentials.Secret, refreshToken);
        var serializerOptios = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        var requestUri = _stravaOptions.BaseUri
            .AppendPathSegment(_stravaOptions.TokenExchangePart)
            .AppendQueryParam("client_id", request.ClientId)
            .AppendQueryParam("client_secret", request.ClientSecret)
            .AppendQueryParam("grant_type", "refresh_token")
            .AppendQueryParam("refresh_token", request.RefreshToken);

        var response = await httpClient.PostAsync(requestUri, null, cancellationToken: token);
        response.EnsureSuccessStatusCode();

        var accessToken = await response.Content.ReadFromJsonAsync<ApiAccessToken>(serializerOptios,token);

        return accessToken;
    }
}
