using TrainingLogger.Infrastructure.Strava;

namespace TrainingLogger.Infrastructure.UnitTests.Strava;

public class StravaOptionsFaker : Faker<StravaOptions>
{
    public StravaOptionsFaker()
    {
        RuleFor(x => x.BaseUri, x => new Uri(x.Internet.Url()));
        RuleFor(x => x.GetActivityByIdPart, x => x.Internet.UrlRootedPath());
        RuleFor(x => x.GetActivitiesPart, x => x.Internet.UrlRootedPath());
        RuleFor(x => x.TokenExchangePart, x => x.Internet.UrlRootedPath());
    }
}