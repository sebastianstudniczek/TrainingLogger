using TrainingLogger.Core.DTOs;

namespace TrainingLogger.Core.Contracts;

public interface IActivitiesClient
{
    Task<ActivityDto?> GetActivityByIdAsync(long id, CancellationToken token);
    Task<IEnumerable<ActivityDto>> GetActivitiesAsync(DateTimeOffset? from = null, CancellationToken token = default);
}
