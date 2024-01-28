using Microsoft.AspNetCore.Mvc;
using TrainingLogger.Core.Notifications.ActivityPublished;
using TrainingLogger.Infrastructure.Notifications;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Web.Endpoints;

public class PostEventEndpoint : IEndpoint
{
    public string Pattern => "/strava-webhook";
    public HttpMethod Method => HttpMethod.Post;
    public Delegate Handler => PostEvent;

    public static async Task<IResult> PostEvent(
        EventDataRequest request,
        [FromServices] INotificationDispatcher notifier,
        CancellationToken token)
    {
        ActivityPublishedNotification eventNotification = new(request.ObjectId);
        await notifier.PublishAsync(eventNotification, token);

        return Results.Ok();
    }
}
