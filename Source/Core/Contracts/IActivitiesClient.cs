using TrainingLogger.Core.DTOs;

namespace TrainingLogger.Core.Contracts;

public interface IActivitiesClient
{
    Task<ActivityDto?> GetActivityByIdAsync(ulong id, CancellationToken token);
}
