using System.Text.Json.Serialization;

namespace TrainingLogger.Core.Models;

public class ActivitySummary
{
    [JsonPropertyName("resource_state")]
    public int? ResourceState { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("distance")]
    public double? Distance { get; init; }

    [JsonPropertyName("moving_time")]
    public int? MovingTime { get; init; }

    [JsonPropertyName("elapsed_time")]
    public int? ElapsedTime { get; init; }

    [JsonPropertyName("total_elevation_gain")]
    public int? TotalElevationGain { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("sport_type")]
    public required string SportType { get; init; }

    [JsonPropertyName("workout_type")]
    public required object WorkoutType { get; init; } // TODO: Object?

    [JsonPropertyName("id")]
    public long? Id { get; init; }

    [JsonPropertyName("external_id")]
    public required string ExternalId { get; init; }

    [JsonPropertyName("upload_id")]
    public required string UploadId { get; init; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("start_date_local")]
    public DateTime? StartDateLocal { get; init; }

    [JsonPropertyName("timezone")]
    public required string Timezone { get; init; }

    [JsonPropertyName("utc_offinit")]
    public int? UtcOffinit { get; init; }

    [JsonPropertyName("start_latlng")]
    public required object StartLatlng { get; init; }

    [JsonPropertyName("end_latlng")]
    public required object EndLatlng { get; init; }

    [JsonPropertyName("location_city")]
    public required object LocationCity { get; init; }

    [JsonPropertyName("location_state")]
    public required object LocationState { get; init; }

    [JsonPropertyName("location_country")]
    public required string LocationCountry { get; init; }

    [JsonPropertyName("achievement_count")]
    public int? AchievementCount { get; init; }

    [JsonPropertyName("kudos_count")]
    public int? KudosCount { get; init; }

    [JsonPropertyName("comment_count")]
    public int? CommentCount { get; init; }

    [JsonPropertyName("athlete_count")]
    public int? AthleteCount { get; init; }

    [JsonPropertyName("trainer")]
    public bool? Trainer { get; init; }

    [JsonPropertyName("commute")]
    public bool? Commute { get; init; }

    [JsonPropertyName("manual")]
    public bool? Manual { get; init; }

    [JsonPropertyName("private")]
    public bool? Private { get; init; }

    [JsonPropertyName("flagged")]
    public bool? Flagged { get; init; }

    [JsonPropertyName("gear_id")]
    public required string GearId { get; init; }

    [JsonPropertyName("from_accepted_tag")]
    public bool? FromAcceptedTag { get; init; }

    [JsonPropertyName("average_speed")]
    public double? AverageSpeed { get; init; }

    [JsonPropertyName("max_speed")]
    public int? MaxSpeed { get; init; }

    [JsonPropertyName("average_cadence")]
    public double? AverageCadence { get; init; }

    [JsonPropertyName("average_watts")]
    public double? AverageWatts { get; init; }

    [JsonPropertyName("weighted_average_watts")]
    public int? WeightedAverageWatts { get; init; }

    [JsonPropertyName("kilojoules")]
    public double? Kilojoules { get; init; }

    [JsonPropertyName("device_watts")]
    public bool? DeviceWatts { get; init; }

    [JsonPropertyName("has_heartrate")]
    public bool? HasHeartrate { get; init; }

    [JsonPropertyName("average_heartrate")]
    public double? AverageHeartrate { get; init; }

    [JsonPropertyName("max_heartrate")]
    public int? MaxHeartrate { get; init; }

    [JsonPropertyName("max_watts")]
    public int? MaxWatts { get; init; }

    [JsonPropertyName("pr_count")]
    public int? PrCount { get; init; }

    [JsonPropertyName("total_photo_count")]
    public int? TotalPhotoCount { get; init; }

    [JsonPropertyName("has_kudoed")]
    public bool? HasKudoed { get; init; }

    [JsonPropertyName("suffer_score")]
    public int? SufferScore { get; init; }
}


