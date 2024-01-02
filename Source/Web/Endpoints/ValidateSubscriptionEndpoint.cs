using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Web.Endpoints;

public class ValidateSubscriptionEndpoint : IEndpoint
{
    public string Pattern => "/strava-webhook";
    public HttpMethod Method => HttpMethod.Get;
    public Delegate Handler => ValidateSubscription;

    public static IResult ValidateSubscription(
        SubscriptionValidationRequest request,
        [FromServices] IOptions<StravaOptions> stravaOptions)
    {
        if (request.Mode != Consts.SubscribeMode || request.Token != stravaOptions.Value.WebhookVerifyToken)
        {
            return Results.Forbid();
        }

        var response = new SubscriptionValidationResponse
        {
            Challenge = request.Challenge
        };
        return TypedResults.Ok(response);
    }
}
