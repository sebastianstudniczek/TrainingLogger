using Bogus;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Core.Models;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Implementations;
using TrainingLogger.Shared.Tests;

namespace TrainingLogger.Infrastructure.UnitTests.Strava;

public class ActivitiesClientTests
{
    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly IOptions<StravaOptions> _options = Substitute.For<IOptions<StravaOptions>>();
    private readonly TrainingLoggerFixture _fixture = new();
    private readonly StravaOptionsFaker _optionsFaker = new();

    [Fact]
    public async Task GetActivityById_ShouldGet_StravaHttpClient()
    {
        var activity = _fixture.Create<ActivityDto>();
        var httpResponse = JsonContent.Create(activity);
        var messageHandler = new MockHttpMessageHandler(
            HttpStatusCode.OK,
            httpResponse);
        var sampleHttpClient = new HttpClient(messageHandler)
        {
            BaseAddress = new Uri("https://www.google.com")
        };
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(sampleHttpClient);
        _fixture.Inject(_httpClientFactory);
        var sampleOptions = _fixture.Create<StravaOptions>();
        string expectedClientName = sampleOptions.HttpClientName;
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
    public async Task GetActivityById_ShouldInvokeHttpClient_WithCorrectRequestPath()
    {
        var sampleOptions = _optionsFaker.Generate();
        _options.Value.Returns(sampleOptions);
        _fixture.Inject(_options);
        var activity = _fixture.Create<ActivityDto>();
        var responseContent = JsonContent.Create(activity);
        ulong activityId = activity.Id;
        var expectedUri = Url
            .Combine(sampleOptions.BaseUri, sampleOptions.GetActivityByIdUri)
            .AppendPathSegment(activityId)
            .ToUri();
        var messageHandler = new MockHttpMessageHandler(
            HttpStatusCode.OK,
            responseContent);
        var sampleHttpClient = new HttpClient(messageHandler)
        {
            BaseAddress = new Uri(sampleOptions.BaseUri)
        };
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(sampleHttpClient);
        _fixture.Inject(_httpClientFactory);
        var activitiesClient = _fixture.Create<ActivitiesClient>();

        _ = await activitiesClient.GetActivityByIdAsync(activityId, default);

        var invokedWith = messageHandler.InvokedWithRequest;
        invokedWith.Should().NotBeNull();
        invokedWith!.RequestUri.Should().BeEquivalentTo(expectedUri);
    }

    [Fact]
    public async Task GetActivityById_ShouldReturnNull_IfRequestWasNotSucceded()
    {
        var options = _optionsFaker.Generate();
        _options.Value.Returns(options);
        var messageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest);
        var httpClient = new HttpClient(messageHandler)
        {
            BaseAddress = new Uri(options.BaseUri)
        };
        _httpClientFactory
            .CreateClient(Arg.Any<string>())
            .Returns(httpClient);
        _fixture.Inject(_httpClientFactory);
        _fixture.Inject(_options);
        var client = _fixture.Create<ActivitiesClient>();

        var actualResult = await client.GetActivityByIdAsync(12, default);

        actualResult.Should().BeNull();
    }


    private sealed class StravaOptionsFaker : Faker<StravaOptions>
    {
        public StravaOptionsFaker()
        {
            RuleFor(x => x.HttpClientName, x => x.Internet.DomainName());
            RuleFor(x => x.BaseUri, x => x.Internet.Url());
            RuleFor(x => x.GetActivityByIdUri, x => x.Internet.UrlRootedPath());
        }
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _responseCode;
        private readonly HttpContent? _responsContent;
        public HttpRequestMessage? InvokedWithRequest { get; private set; }

        public MockHttpMessageHandler(HttpStatusCode responseCode = HttpStatusCode.OK, HttpContent? responseContent = null)
        {
            _responseCode = responseCode;
            _responsContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            InvokedWithRequest = request;
            return _responsContent is null
                ? Task.FromResult(new HttpResponseMessage(_responseCode))
                : Task.FromResult(new HttpResponseMessage(_responseCode)
                {
                    Content = _responsContent
                });
        }
    }
}