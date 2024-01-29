using System.Text.Json.Serialization;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Core.DTOs;

public class ActivityDto
{
    [JsonPropertyName("id")]
    public ulong Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("distance")]
    public int Distance { get; init; }

    [JsonPropertyName("moving_time")]
    public int MovingTime { get; init; }

    [JsonPropertyName("elapsed_time")]
    public int ElapsedTime { get; init; }

    [JsonPropertyName("sport_type")]
    public required SportType SportType { get; init; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; init; }

    [JsonPropertyName("start_date_local")]
    public DateTime StartDateLocal { get; init; }

    [JsonPropertyName("average_cadence")]
    public double AverageCadence { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("calories")]
    public double Calories { get; init; }
}
