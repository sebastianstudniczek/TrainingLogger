using TrainingLogger.Core.Models;

namespace TrainingLogger.Core.Contracts;

public interface IActivitiesClient {
    Task<Activity?> GetActivityByIdAsync(ulong id, CancellationToken token);
}
