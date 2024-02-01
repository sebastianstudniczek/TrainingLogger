using TrainingLogger.Core.Models;

namespace TrainingLogger.Core.DTOs;

internal static class MappingExtensions
{
    public static Activity AsEntity(this ActivityDto dto)
        => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Distance = dto.Distance,
            MovingTime = dto.MovingTime,
            ElapsedTime = dto.ElapsedTime,
            SportType = dto.SportType,
            StartDate = dto.StartDate,
            StartDateLocal = dto.StartDateLocal,
            Description = dto.Description,
            Calories = dto.Calories
        };
}