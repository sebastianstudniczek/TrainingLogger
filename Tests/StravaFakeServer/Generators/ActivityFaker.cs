using Bogus;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Core.Models;

namespace TrainingLogger.StravaFakeServer.Generators;

public sealed class ActivityFaker : Faker<ActivityDto>
{
    public ActivityFaker()
    {
        RuleFor(x => x.Id, x => x.Random.Number(999));
        RuleFor(x => x.Name, x => $"{x.Address.City()} Running {x.Random.Number(1, 25)} km");
        RuleFor(x => x.Distance, x => x.Random.Number(50));
        RuleFor(x => x.MovingTime, x => x.Random.Number(5));
        RuleFor(x => x.SportType, x => x.PickRandom<SportType>());
        RuleFor(x => x.StartDate, x => x.Date.Recent());
        RuleFor(x => x.StartDateLocal, x => x.Date.Recent());
        RuleFor(x => x.Description, x => $"Training: {x.Random.Word()}/{x.Random.Number(10)}");
        RuleFor(x => x.Calories, x => x.Random.Number(100, 1200));
    }

    public ActivityFaker WithId(long id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }
}