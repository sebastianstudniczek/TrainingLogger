using Flurl;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Shared.Tests;
using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;

namespace TrainingLogger.Infrastructure.UnitTests;

public class StravaClientTests
{
    private readonly ITokenStore _tokenStore = Substitute.For<ITokenStore>();
    private readonly IFixture _fixture = new TrainingLoggerFixture();
    private readonly StravaClient _client;
    private readonly IOptions<StravaOptions> _stravaOptions;

    public StravaClientTests()
    {
        var defaultOptions = new StravaOptions()
        {
            BaseUri = "https://somepage.com",
            GetActivityByIdUri = "api/act/{0}",
            GetActivitiesUri = "activs"
        };
        _stravaOptions = Options.Create(defaultOptions);

        _client = new StravaClient(_stravaOptions, _tokenStore);
    }

    [Fact]
    public async Task GetActivityAsync_ShouldGet_Acivity_WithGivenId()
    {
        int expectedId = TestUtils.RandomInt;
        using var httpTest = new HttpTest();

        _ = await _client.GetActivityAsync(expectedId, default);

        string expectedUrl = Url.Combine(
            _stravaOptions.Value.BaseUri, 
            string.Format(_stravaOptions.Value.GetActivityByIdUri, expectedId));
        httpTest.ShouldHaveCalled(expectedUrl);
    }

    [Fact]
    public async Task GetActivityAsync_ShouldGet_Activity_WithBearerToken()
    {
        using var httpTest = new HttpTest();
        _tokenStore
            .GetTokenAsync(Arg.Any<CancellationToken>())
            .Returns(TestUtils.RandomString);

        _ = await _client.GetActivityAsync(12, default);

        httpTest
            .ShouldHaveMadeACall()
            .WithOAuthBearerToken();
    }

    [Fact]
    public async Task GetActivityAsync_ShouldGet_Activity_WithBearerToken_TakenFromTokenStore()
    {
        string expectedToken = TestUtils.RandomString;
        _tokenStore
            .GetTokenAsync(Arg.Any<CancellationToken>())
            .Returns(expectedToken);
        using var httpTest = new HttpTest();

        _ = await _client.GetActivityAsync(2, default);

        await _tokenStore.Received(1).GetTokenAsync(Arg.Any<CancellationToken>());
        httpTest
            .ShouldHaveMadeACall()
            .WithOAuthBearerToken(expectedToken);
    }

    [Fact]
    public async Task GetActivityAsync_ShouldReturn_FetchedActivity()
    {
        int expectedId = TestUtils.RandomInt;
        using var httpTest = new HttpTest();
        var expectedActivity = _fixture.Create<Activity>();
        httpTest.RespondWithJson(expectedActivity);

        var actualActivity = await _client.GetActivityAsync(expectedId, default);

        actualActivity.Should().BeEquivalentTo(expectedActivity);
    }

    [Fact]
    public async Task GetActivitySummariesAsync_ShoulGet_ActivitySummaries()
    {
        using var httpTest = new HttpTest();
        string expectedUrl = Url.Combine(_stravaOptions.Value.BaseUri, _stravaOptions.Value.GetActivitiesUri);

        _ = await _client.GetActivitySummariesAsync(default);

        httpTest
            .ShouldHaveCalled(expectedUrl)
            .WithVerb(HttpMethod.Get);
    }
}