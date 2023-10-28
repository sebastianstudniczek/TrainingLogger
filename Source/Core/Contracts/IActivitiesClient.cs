using TrainingLogger.Core.Models;

namespace TrainingLogger.Core.Contracts;

internal interface IActivitiesClient {
    Task<Activity> GetActivityByIdAsync(ulong id, CancellationToken token);
}
