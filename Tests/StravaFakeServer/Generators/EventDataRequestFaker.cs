using Bogus;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.StravaFakeServer.Generators;

public class EventDataRequestFaker : Faker<EventDataRequest>
{
    public EventDataRequestFaker()
    {
        RuleFor(x => x.ObjectId, x => (ulong)x.Random.Long());
        RuleFor(x => x.EventTime, (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        RuleFor(x => x.SubscriptionId, x => (ulong)x.Random.Long());
    }
}