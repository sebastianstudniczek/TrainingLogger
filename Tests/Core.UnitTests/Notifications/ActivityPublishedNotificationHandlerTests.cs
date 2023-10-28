using AutoFixture.AutoNSubstitute;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.Models;
using TrainingLogger.Core.Notifications.ActivityPublished;

namespace TrainingLogger.Core.UnitTests.Notifications;

public class ActivityPublishedNotificationHandlerTests {
    private readonly IApplicationDbContext _dbContext = Substitute.For<IApplicationDbContext>();
    private readonly IActivitiesClient _activitiesClient = Substitute.For<IActivitiesClient>();
    private readonly IFixture _fixture = new Fixture();

    public ActivityPublishedNotificationHandlerTests() {
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task ShouldGet_Activity_ById_TakenFromNotification() {
        var notification = _fixture.Create<ActivityPublishedNotification>();
        _fixture.Inject(_activitiesClient);
        var handler = _fixture.Create<ActivityPublishedNotificationHandler>();

        await handler.Handle(notification, default);

        await _activitiesClient
            .Received()
            .GetActivityByIdAsync(notification.ActivityId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ShouldSave_DownloadedActivity() {
        var notification = _fixture.Create<ActivityPublishedNotification>();
        var activity = _fixture.Create<Activity>();
        _activitiesClient
            .GetActivityByIdAsync(Arg.Any<ulong>(), Arg.Any<CancellationToken>())
            .Returns(activity);
        _fixture.Inject(_dbContext);
        _fixture.Inject(_activitiesClient);
        var handler = _fixture.Create<ActivityPublishedNotificationHandler>();

        await handler.Handle(notification, default);

        _dbContext
            .Activities
            .Received()
            .Add(activity);
        await _dbContext
            .Received()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
