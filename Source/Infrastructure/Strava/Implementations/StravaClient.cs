using Microsoft.Extensions.Options;
using TrainingLogger.Core.Models;
using Flurl;
using Flurl.Http;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class StravaClient : IStravaClient
{
    private readonly ITokenStore _tokenStore;
    private readonly StravaOptions _options;

    public StravaClient(IOptions<StravaOptions> options, ITokenStore tokenStore)
    {
        _options = options.Value;
        _tokenStore = tokenStore;
    }

    public async Task<Activity> GetActivityAsync(long id, CancellationToken cancellationToken)
    {
        string jwtToken = await _tokenStore.GetTokenAsync(GetTokenAsync, cancellationToken);

        return await _options.BaseUri
            .AppendPathSegment(string.Format(_options.GetActivityByIdUri, id))
            .WithOAuthBearerToken(jwtToken)
            .GetJsonAsync<Activity>(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ActivitySummary>> GetActivitySummariesAsync(CancellationToken cancellationToken)
    {
        string jwtToken = await _tokenStore.GetTokenAsync(GetTokenAsync, cancellationToken);

        return await _options.BaseUri
            .AppendPathSegment(_options.GetActivitiesUri)
            .WithOAuthBearerToken(jwtToken)
            .GetJsonAsync<IReadOnlyList<ActivitySummary>>(cancellationToken);
    }

    private Task<ApiAccessToken> GetTokenAsync(string refreshToken)
    {
        return Task.FromResult(new ApiAccessToken());
    }
}
