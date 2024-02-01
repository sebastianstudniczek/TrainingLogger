using Bogus;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.StravaFakeServer.Generators;

public class EventDataRequestFaker : Faker<EventDataRequest>
{
    public EventDataRequestFaker()
    {
        RuleFor(x => x.ObjectId, x => x.Random.Long());
        RuleFor(x => x.EventTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        RuleFor(x => x.SubscriptionId, x => x.Random.Long());
    }
}