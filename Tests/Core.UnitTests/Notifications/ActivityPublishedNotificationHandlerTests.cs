using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Core.Models;
using TrainingLogger.Core.Notifications.ActivityPublished;

namespace TrainingLogger.Core.UnitTests.Notifications;

public class ActivityPublishedNotificationHandlerTests {
    private readonly IApplicationDbContext _dbContext = Substitute.For<IApplicationDbContext>();
    private readonly IActivitiesClient _activitiesClient = Substitute.For<IActivitiesClient>();
    private readonly TrainingLoggerFixture _fixture = new();

    [Fact]
    public async Task Should_Get_Activity_By_Id_Taken_From_Notification() {
        var notification = _fixture.Create<ActivityPublishedNotification>();
        _fixture.Inject(_activitiesClient);
        var handler = _fixture.Create<ActivityPublishedNotificationHandler>();

        await handler.HandleAsync(notification, default);

        await _activitiesClient
            .Received()
            .GetActivityByIdAsync(notification.ActivityId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_Save_Downloaded_Activity() {
        var notification = _fixture.Create<ActivityPublishedNotification>();
        var activityDto = _fixture.Create<ActivityDto>();
        _activitiesClient
            .GetActivityByIdAsync(Arg.Any<ulong>(), Arg.Any<CancellationToken>())
            .Returns(activityDto);
        Activity? invokedWith = null;
        _dbContext
            .Activities
            .Add(Arg.Do<Activity>(x => invokedWith = x));
        _fixture.Inject(_dbContext);
        _fixture.Inject(_activitiesClient);
        var handler = _fixture.Create<ActivityPublishedNotificationHandler>();

        await handler.HandleAsync(notification, default);

        invokedWith.Should().NotBeNull();
        invokedWith.Should().BeEquivalentTo(activityDto.AsEntity());
        await _dbContext
            .Received()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
