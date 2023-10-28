using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shared.Tests;
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
        var webhookOptions = new StravaWebhookOptions
        {
            VerifyToken = TestUtils.RandomString
        };
        var invalidRequest = _fixture
            .Build<SubscriptionValidationRequest>()
            .With(x => x.Token, webhookOptions.VerifyToken)
            .Create();

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ShouldReturn_ForbidResult_IfVerifyToken_IsNotValid()
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

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(invalidRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<ForbidHttpResult>();
    }

    [Fact]
    public void ShouldReturn_OkResult_WithReceivedChallange_IfModeAndToken_AreValid()
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

        var result = ValidateSubscriptionEndpoint.ValidateSubscription(validRequest, Options.Create(webhookOptions));

        result.Should().BeOfType<Ok<SubscriptionValidationResponse>>();
        var response = result as Ok<SubscriptionValidationResponse>;
        response?.Value?.Challenge.Should().Be(validRequest.Challenge);
    }
}
