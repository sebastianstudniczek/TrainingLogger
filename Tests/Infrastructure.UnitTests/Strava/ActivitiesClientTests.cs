using Flurl;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;

namespace TrainingLogger.Infrastructure.UnitTests.Strava;

public class ActivitiesClientTests
{
    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly IOptions<StravaOptions> _options = Substitute.For<IOptions<StravaOptions>>();
    private readonly TrainingLoggerFixture _fixture = new();
    private readonly StravaOptionsFaker _optionsFaker = new();

    [Fact]
    public async Task Get_Activity_By_Id_Should_Use_Strava_Http_Client()
    {
        var activity = _fixture.Create<ActivityDto>();
        var mockHttpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(activity))
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(mockHttpClient.Client);
        _fixture.Inject(_httpClientFactory);
        var sampleOptions = _fixture.Create<StravaOptions>();
        const string expectedClientName = "Strava";
        _options.Value.Returns(sampleOptions);
        _fixture.Inject(_options);
        var client = _fixture.Create<ActivitiesClient>();
        ulong acitivtyId = _fixture.Create<ulong>();

        _ = await client.GetActivityByIdAsync(acitivtyId, default);

        _httpClientFactory
            .Received()
            .CreateClient(expectedClientName);
    }

    [Fact]
    public async Task GetActivityById_Should_Get_Activity_From_Strava()
    {
        var sampleOptions = _optionsFaker.Generate();
        _options.Value.Returns(sampleOptions);
        _fixture.Inject(_options);
        var activity = _fixture.Create<ActivityDto>();
        ulong activityId = activity.Id;
        var expectedUri = Url
            .Combine(sampleOptions.BaseUri.ToString(), sampleOptions.GetActivityByIdPart)
            .AppendPathSegment(activityId)
            .ToUri();
        var mockHttpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(activity))
            .WithBaseUri(sampleOptions.BaseUri)
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(mockHttpClient.Client);
        _fixture.Inject(_httpClientFactory);
        var activitiesClient = _fixture.Create<ActivitiesClient>();

        _ = await activitiesClient.GetActivityByIdAsync(activityId, default);

        var invokedWith = mockHttpClient.MessageHandler.InvokedWithRequest;
        invokedWith.Should().NotBeNull();
        invokedWith!.RequestUri.Should().BeEquivalentTo(expectedUri);
    }

    [Fact]
    public async Task Get_ActivityById_Should_Return_Null_If_Request_Was_Not_Successful()
    {
        var sampleOptions = _optionsFaker.Generate();
        _options.Value.Returns(sampleOptions);
        _fixture.Inject(_options);
        var httpClient = new MockHttpClientBuilder()
            .WithResponseCode(HttpStatusCode.BadRequest)
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(httpClient.Client);
        _fixture.Inject(_httpClientFactory);
        _fixture.Inject(_options);
        var client = _fixture.Create<ActivitiesClient>();

        var actualResult = await client.GetActivityByIdAsync(12, default);

        actualResult.Should().BeNull();
    }
}