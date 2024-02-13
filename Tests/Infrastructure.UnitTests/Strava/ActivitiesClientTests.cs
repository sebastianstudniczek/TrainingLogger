using Flurl;
using Microsoft.Extensions.Options;
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
    public async Task GetActivity_By_Id_Should_Use_Strava_Http_Client()
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
        long acitivtyId = _fixture.Create<long>();

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
        long activityId = activity.Id;
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
    public async Task GetActivities_Should_Use_Strava_Http_Client()
    {
        var activity = _fixture.CreateMany<ActivityDto>();
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

        _ = await client.GetActivitiesAsync(default);

        _httpClientFactory
            .Received()
            .CreateClient(expectedClientName);
    }

    [Fact]
    public async Task GetActivities_Should_Get_Activities_From_Strava()
    {
        var sampleOptions = _optionsFaker.Generate();
        _options.Value.Returns(sampleOptions);
        _fixture.Inject(_options);
        var activity = _fixture.CreateMany<ActivityDto>(3);
        var expectedUri = sampleOptions.BaseUri.AppendPathSegment(sampleOptions.GetActivitiesPart).ToString();
        var mockHttpClient = new MockHttpClientBuilder()
            .WithReponseContent(JsonContent.Create(activity))
            .WithBaseUri(sampleOptions.BaseUri)
            .Build();
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(mockHttpClient.Client);
        _fixture.Inject(_httpClientFactory);
        var activitiesClient = _fixture.Create<ActivitiesClient>();

        _ = await activitiesClient.GetActivitiesAsync(default);

        var invokedWith = mockHttpClient.MessageHandler.InvokedWithRequest;
        invokedWith.Should().NotBeNull();
        invokedWith!.RequestUri!.ToString().Should().BeEquivalentTo(expectedUri);
    }
}