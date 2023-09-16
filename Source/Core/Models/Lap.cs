using System.Text.Json.Serialization;

namespace TrainingLogger.Core.Models;

public class Lap
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("resource_state")]
    public int? ResourceState { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("activity")]
    public required Activity Activity { get; set; }

    [JsonPropertyName("elapsed_time")]
    public int? ElapsedTime { get; set; }

    [JsonPropertyName("moving_time")]
    public int? MovingTime { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("start_date_local")]
    public DateTime? StartDateLocal { get; set; }

    [JsonPropertyName("distance")]
    public double? Distance { get; set; }

    [JsonPropertyName("start_index")]
    public int? StartIndex { get; set; }

    [JsonPropertyName("end_index")]
    public int? EndIndex { get; set; }

    [JsonPropertyName("total_elevation_gain")]
    public int? TotalElevationGain { get; set; }

    [JsonPropertyName("average_speed")]
    public double? AverageSpeed { get; set; }

    [JsonPropertyName("max_speed")]
    public double? MaxSpeed { get; set; }

    [JsonPropertyName("average_cadence")]
    public double? AverageCadence { get; set; }

    [JsonPropertyName("device_watts")]
    public bool? DeviceWatts { get; set; }

    [JsonPropertyName("average_watts")]
    public double? AverageWatts { get; set; }

    [JsonPropertyName("lap_index")]
    public int? LapIndex { get; set; }

    [JsonPropertyName("split")]
    public int? Split { get; set; }
}