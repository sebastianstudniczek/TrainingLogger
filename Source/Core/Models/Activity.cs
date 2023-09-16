using System.Text.Json.Serialization;

namespace TrainingLogger.Core.Models;

public class Activity
{
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    [JsonPropertyName("resource_state")]
    public int? ResourceState { get; init; }

    [JsonPropertyName("external_id")]
    public required string ExternalId { get; init; }

    [JsonPropertyName("upload_id")]
    public long? UploadId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("distance")]
    public int? Distance { get; init; }

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

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("start_date_local")]
    public DateTime? StartDateLocal { get; init; }

    [JsonPropertyName("timezone")]
    public required string Timezone { get; init; }

    [JsonPropertyName("utc_offinit")]
    public int? UtcOffinit { get; init; }

    [JsonPropertyName("start_latlng")]
    public IReadOnlyList<double?> StartLatlng { get; init; } = new List<double?>();

    [JsonPropertyName("end_latlng")]
    public IReadOnlyList<double?> EndLatlng { get; init; } = new List<double?>();

    [JsonPropertyName("achievement_count")]
    public int? AchievementCount { get; init; }

    [JsonPropertyName("kudos_count")]
    public int? KudosCount { get; init; }

    [JsonPropertyName("comment_count")]
    public int? CommentCount { get; init; }

    [JsonPropertyName("athlete_count")]
    public int? AthleteCount { get; init; }

    [JsonPropertyName("photo_count")]
    public int? PhotoCount { get; init; }

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

    [JsonPropertyName("from_accepted_tag")]
    public bool? FromAcceptedTag { get; init; }

    [JsonPropertyName("average_speed")]
    public double? AverageSpeed { get; init; }

    [JsonPropertyName("max_speed")]
    public double? MaxSpeed { get; init; }

    [JsonPropertyName("average_cadence")]
    public double? AverageCadence { get; init; }

    [JsonPropertyName("average_temp")]
    public int? AverageTemp { get; init; }

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

    [JsonPropertyName("max_watts")]
    public int? MaxWatts { get; init; }

    [JsonPropertyName("elev_high")]
    public double? ElevHigh { get; init; }

    [JsonPropertyName("elev_low")]
    public double? ElevLow { get; init; }

    [JsonPropertyName("pr_count")]
    public int? PrCount { get; init; }

    [JsonPropertyName("total_photo_count")]
    public int? TotalPhotoCount { get; init; }

    [JsonPropertyName("has_kudoed")]
    public bool? HasKudoed { get; init; }

    [JsonPropertyName("workout_type")]
    public int? WorkoutType { get; init; }

    [JsonPropertyName("suffer_score")]
    public required object SufferScore { get; init; }// TODO: Przerobić

    [JsonPropertyName("description")]
    public string Description { get; init; } = default!;

    [JsonPropertyName("calories")]
    public double? Calories { get; init; }

    [JsonPropertyName("splits_metric")]
    public IReadOnlyList<SplitsMetric> SplitsMetric { get; init; } = new List<SplitsMetric>();

    [JsonPropertyName("laps")]
    public IReadOnlyList<Lap> Laps { get; init; } = new List<Lap>();

    [JsonPropertyName("device_name")]
    public string DeviceName { get; init; } = default!;

    [JsonPropertyName("embed_token")]
    public string EmbedToken { get; init; } = default!;

    [JsonPropertyName("segment_leaderboard_opt_out")]
    public bool? SegmentLeaderboardOptOut { get; init; }

    [JsonPropertyName("leaderboard_opt_out")]
    public bool? LeaderboardOptOut { get; init; }
}