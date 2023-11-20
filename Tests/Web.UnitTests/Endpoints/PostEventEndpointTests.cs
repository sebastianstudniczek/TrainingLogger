using Microsoft.AspNetCore.Http.HttpResults;
using TrainingLogger.Core.Notifications.ActivityPublished;
using TrainingLogger.Infrastructure.Notifications;
using TrainingLogger.Infrastructure.Strava.Models;
using TrainingLogger.Web.Endpoints;

namespace TrainingLogger.Web.UnitTests.Endpoints;

public class PostEventEndpointTests
{
    private readonly INotificationDispatcher _mediator = Substitute.For<INotificationDispatcher>();
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task ShouldReturnOkResult()
    {
        var dataRequest = _fixture.Create<EventDataRequest>();

        var result = await PostEventEndpoint.PostEvent(dataRequest, _mediator, default);

        result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task ShouldPublishEvent_AboutReceivedEvent()
    {
        var dataRequest = _fixture.Create<EventDataRequest>();

        _ = await PostEventEndpoint.PostEvent(dataRequest, _mediator, default);

        await _mediator
            .Received()
            .PublishAsync(Arg.Any<ActivityPublishedNotification>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldPublishEvent_AboutReceivedEvent_WithActivityId()
    {
        var dataRequest = _fixture.Create<EventDataRequest>();
        var expectedNotification = _fixture
            .Build<ActivityPublishedNotification>()
            .With(x => x.ActivityId, dataRequest.ObjectId)
            .Create();
        ActivityPublishedNotification? invokedWith = null;
        await _mediator.PublishAsync(Arg.Do<ActivityPublishedNotification>(x => invokedWith = x), Arg.Any<CancellationToken>());

        _ = PostEventEndpoint.PostEvent(dataRequest, _mediator, default);

        invokedWith.Should().NotBeNull();
        invokedWith.Should().BeEquivalentTo(expectedNotification);
    }
}
