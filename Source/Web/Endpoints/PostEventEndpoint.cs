using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingLogger.Core.Notifications.ActivityPublished;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Web.Endpoints;

public class PostEventEndpoint : IEndpoint
{
    public string Pattern => "/strava-webhook";
    public HttpMethod Method => HttpMethod.Get;
    public Delegate Handler => PostEvent;

    public static async Task<IResult> PostEvent(
        EventDataRequest request,
        [FromServices] IMediator mediator,
        CancellationToken token)
    {
        ActivityPublishedNotification eventNotification = new(request.ObjectId);
        await mediator.Publish(eventNotification, token);

        return Results.Ok();
    }
}
