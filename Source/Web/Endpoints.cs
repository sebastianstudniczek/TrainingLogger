using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrainingLogger.Infrastructure.Strava;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Web;

public static class Endpoints
{
    public static IResult PostEvent(EventDataRequest request)
    {
        return Results.Ok();
    }

    public static IResult ValidateSubscription(SubscriptionValidationRequest request, [FromServices] IOptions<StravaWebhookOptions> webhookOptions)
    {
        if (request.Mode != Infrastructure.Strava.Consts.SubscribeMode || request.Token != webhookOptions.Value.VerifyToken)
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
