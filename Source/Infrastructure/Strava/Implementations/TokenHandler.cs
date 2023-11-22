using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using TrainingLogger.Infrastructure.Strava.Interfaces;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class TokenHandler : DelegatingHandler
{
    private readonly ITokenStore _tokenStore;
    private readonly string _authScheme;

    public TokenHandler(ITokenStore tokenStore, IOptions<StravaOptions> options)
    {
        _tokenStore = tokenStore;
        _authScheme = options.Value.AuthScheme;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string authToken = await _tokenStore.GetTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue(_authScheme, authToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
