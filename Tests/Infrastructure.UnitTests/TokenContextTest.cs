using Shared.Tests;
using System.Net.Http.Headers;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Interfaces;

namespace TrainingLogger.Infrastructure.UnitTests;

public class TokenContextTest
{
    private readonly TokenContext _tokenContext;
    private readonly ITokenStore _tokenStore = Substitute.For<ITokenStore>();
    private readonly GetRefreshedToken _getRefreshedToken = Substitute.For<GetRefreshedToken>();
    private readonly HttpClient _httpClient = new();
    private readonly Func<HttpClient, Task<string>> _sampleRequest = Substitute.For<Func<HttpClient, Task<string>>>();

    public TokenContextTest()
    {
        _tokenContext = new TokenContext(_httpClient, _tokenStore);
    }

    [Fact]
    public async Task ShouldGet_TokenFromTokenStore()
    {
        _ = await _tokenContext.RunAsync(_sampleRequest, _getRefreshedToken, default);

        await _tokenStore
            .Received()
            .GetTokenAsync(Arg.Any<GetRefreshedToken>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldProvide_RefreshTokenMethod_ToTokensStoreGetToken()
    {
        _ = await _tokenContext.RunAsync(_sampleRequest, _getRefreshedToken, default);

        await _tokenStore
            .Received()
            .GetTokenAsync(_getRefreshedToken, default);
    }

    [Fact]
    public async Task ShouldAssign_TokenReceived_FromTokenStore_ToHttpClientHeaderAsBearerToken()
    {
        string bearerToken = TestUtils.RandomString;
        _tokenStore
            .GetTokenAsync(Arg.Any<GetRefreshedToken>(), Arg.Any<CancellationToken>())
            .Returns(bearerToken);
        var expectedAuthHeader = new AuthenticationHeaderValue("Bearer", bearerToken);

        _ = await _tokenContext.RunAsync(_sampleRequest, _getRefreshedToken, default);

        _httpClient
            .DefaultRequestHeaders
            .Authorization
            .Should()
            .BeEquivalentTo(expectedAuthHeader);
    }

    [Fact]
    public async Task ShouldInvoke_GivenRequest_WithProvidedHttpClient()
    {
        _ = await _tokenContext.RunAsync(_sampleRequest, _getRefreshedToken, default);

        await _sampleRequest
            .Received()
            .Invoke(_httpClient);
    }
}
