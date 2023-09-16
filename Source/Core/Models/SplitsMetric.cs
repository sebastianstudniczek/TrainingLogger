using System.Text.Json.Serialization;

namespace TrainingLogger.Core.Models;

public class SplitsMetric
{
    [JsonPropertyName("distance")]
    public double? Distance { get; init; }

    [JsonPropertyName("elapsed_time")]
    public int? ElapsedTime { get; init; }

    [JsonPropertyName("elevation_difference")]
    public double? ElevationDifference { get; init; }

    [JsonPropertyName("moving_time")]
    public int? MovingTime { get; init; }

    [JsonPropertyName("split")]
    public int? Split { get; init; }

    [JsonPropertyName("average_speed")]
    public double? AverageSpeed { get; init; }

    [JsonPropertyName("pace_zone")]
    public int? PaceZone { get; init; }
}