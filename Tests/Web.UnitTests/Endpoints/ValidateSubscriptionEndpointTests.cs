using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Models;
using TrainingLogger.Web.Endpoints;

namespace TrainingLogger.Web.UnitTests.Endpoints;

public class ValidateSubscriptionEndpointTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void ShouldReturn_ForbiddenResult_IfRequestMode_IsNotSubscription()
    {
        var webhookOptions = new StravaOptions
        {
            WebhookVerifyToken = TestUtils.RandomString
        };
        var invalidRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Token, webhookOptions.WebhookVerifyToken)
            .Create();

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ShouldReturn_ForbidResult_IfWebhookVerifyToken_IsNotValid()
    {
        var webhookOptions = new StravaOptions
        {
            WebhookVerifyToken = TestUtils.RandomString
        };
        const string validMode = "subscribe";
        var invalidRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Mode, validMode)
            .Create();

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ShouldReturn_OkResult_WithReceivedChallange_IfModeAndToken_AreValid()
    {
        var webhookOptions = new StravaOptions
        {
            WebhookVerifyToken = TestUtils.RandomString
        };
        const string validMode = "subscribe";
        var validRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Mode, validMode)
            .With(x => x.Token, webhookOptions.WebhookVerifyToken)
            .Create();

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(validRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<Ok<SubscriptionValidationResponse>>();
        var response = result as Ok<SubscriptionValidationResponse>;
        response?.Value?.Challenge.Should().Be(validRequest.Challenge);
    }
}
