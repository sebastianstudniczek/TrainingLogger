using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Infrastructure.Strava.Interfaces;
using TrainingLogger.Shared.Tests;

namespace TrainingLogger.Infrastructure.UnitTests.Strava;

public class TokenHandlerTests
{
    private readonly ITokenStore _tokenStore = Substitute.For<ITokenStore>();
    private readonly TrainingLoggerFixture _fixture = new();

    public TokenHandlerTests()
    {
        var sampleOptions = _fixture.Create<StravaOptions>();
        _fixture.Register(() => Options.Create(sampleOptions));
    }

    [Fact]
    public async Task ShouldGetToken_FromTokenStore()
    {
        _fixture.Inject(_tokenStore);
        var handler = _fixture.Create<TokenHandler>();
        handler.InnerHandler = new SuccessHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.google.com");

        _ = await httpClient.SendAsync(requestMessage);

        await _tokenStore
            .Received()
            .GetTokenAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldSetAuthorizationHeaderValue_WithTokenTakenFromTokenStore()
    {
        var options = _fixture.Create<StravaOptions>();
        string expectedToken = _fixture.Create<string>();
        AuthenticationHeaderValue expectedAuthHeader = new(options.AuthScheme, expectedToken);
        _tokenStore
            .GetTokenAsync(Arg.Any<CancellationToken>())
            .Returns(expectedToken);
        var optionsWrapper = Options.Create(options);
        _fixture.Inject(optionsWrapper);
        _fixture.Inject(_tokenStore);
        var handler = _fixture.Create<TokenHandler>();
        handler.InnerHandler = new SuccessHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.google.com");

        _ = await httpClient.SendAsync(requestMessage);

        requestMessage
            .Headers
            .Authorization
            .Should()
            .BeEquivalentTo(expectedAuthHeader);
    }

    [Fact]
    public async Task ShouldInvoke_InnerHandler()
    {
        var handler = _fixture.Create<TokenHandler>();
        var innerHandler = new SuccessHttpMessageHandler();
        handler.InnerHandler = innerHandler;
        var httpClient = new HttpClient(handler);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.google.com");

        _ = await httpClient.SendAsync(requestMessage);

        innerHandler.WasInvoked.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturn_ResponseMessage_ReturnedByInnerHandler()
    {
        var handler = _fixture.Create<TokenHandler>();
        var expectedResponseMessage = _fixture.Create<HttpResponseMessage>();
        var innerHandler = new SuccessHttpMessageHandler(expectedResponseMessage);
        handler.InnerHandler = innerHandler;
        var httpClient = new HttpClient(handler);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.google.com");

        var actualResponseMessage = await httpClient.SendAsync(requestMessage);

        actualResponseMessage.Should().BeEquivalentTo(expectedResponseMessage);
    }
}

public class SuccessHttpMessageHandler : HttpMessageHandler
{
    public bool WasInvoked;
    private readonly HttpResponseMessage? _reponse;

    public SuccessHttpMessageHandler(HttpResponseMessage? response = null)
    {
        _reponse = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        WasInvoked = true;

        return _reponse is null
            ? Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("test")
            })
            : Task.FromResult(_reponse);
    }
}
