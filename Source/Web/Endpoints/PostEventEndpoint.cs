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
        // TODO: Debug this and watch how the flows is
        // Maybe it will be better to create some background process to really decouple this
        // from this request as is should be according to the strava api
        // TODO: Check out with stopwatch how much it takes
        await notifier.PublishAsync(eventNotification, token);

        return Results.Ok();
    }
}
