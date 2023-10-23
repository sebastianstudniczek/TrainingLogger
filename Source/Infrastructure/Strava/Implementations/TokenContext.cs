using System.Net.Http.Headers;
using TrainingLogger.Infrastructure.Strava.Interfaces;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal class TokenContext : ITokenContext
{
    private const string _AuthorizationScheme = "Bearer";
    private readonly HttpClient _httpClient;
    private readonly ITokenStore _tokenStore;

    public TokenContext(HttpClient httpClient, ITokenStore tokenStore)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
    }

    public async Task<TResult> RunAsync<TResult>(
        Func<HttpClient, Task<TResult>> requestToExecute,
        GetRefreshedToken fetchNewToken,
        CancellationToken cancellationToken)
    {
        string token = await _tokenStore.GetTokenAsync(fetchNewToken, cancellationToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_AuthorizationScheme, token);

        return await requestToExecute(_httpClient);
    }
}
