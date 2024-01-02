using TrainingLogger.Shared.Exceptions;

namespace TrainingLogger.Infrastructure.Strava.Exceptions;

internal class StravaAuthTokenNotFound : TrainingLoggerException
{
    public StravaAuthTokenNotFound() : base("No auth token is stored.")
    {
        
    }
}
