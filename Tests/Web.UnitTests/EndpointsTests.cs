using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Shared.Tests;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Models;
using TrainingLogger.Web;

namespace Web.UnitTests;

public class EndpointsTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void ValidateSubscription_ShouldReturn_ForbiddenResult_IfRequestMode_IsNotSubscription()
    {
        var webhookOptions = new StravaWebhookOptions
        {
            VerifyToken = TestUtils.RandomString
        };
        var invalidRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Token, webhookOptions.VerifyToken)
            .Create();

        var result = Endpoints.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ValidateSubscription_ShouldReturn_ForbidResult_IfVerifyToken_IsNotValid()
    {
        var webhookOptions = new StravaWebhookOptions
        {
            VerifyToken = TestUtils.RandomString
        };
        const string validMode = "subscribe";
        var invalidRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Mode, validMode)
            .Create();

        var result = Endpoints.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ValidateSubscription_ShouldReturn_OkResult_WithReceivedChallange_IfModeAndToken_AreValid()
    {
        var webhookOptions = new StravaWebhookOptions
        {
            VerifyToken = TestUtils.RandomString
        };
        const string validMode = "subscribe";
        var validRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Mode, validMode)
            .With(x => x.Token, webhookOptions.VerifyToken)
            .Create();

        var result = Endpoints.ValidateSubscription(validRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<Ok<SubscriptionValidationResponse>>();
        var response = result as Ok<SubscriptionValidationResponse>;
        response?.Value?.Challenge.Should().Be(validRequest.Challenge);
    }
}
