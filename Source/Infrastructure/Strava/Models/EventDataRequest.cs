using System.Text.Json.Serialization;

namespace TrainingLogger.Infrastructure.Strava.Models;

public record EventDataRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("object_type")]
    public required EventObjectType ObjectType { get; init; }

    [JsonPropertyName("object_id")]
    public required ulong ObjectId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("aspect_type")]
    public required EventAspectType AspectType { get; init; }

    [JsonPropertyName("event_time")]
    public ulong EventTime { get; init; }

    [JsonPropertyName("subscription_id")]
    public ulong SubscriptionId { get; init; }

    [JsonPropertyName("updates")]
    public dynamic? Updates { get; init; }
}
