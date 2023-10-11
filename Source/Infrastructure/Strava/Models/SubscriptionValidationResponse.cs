using System.Text.Json.Serialization;

namespace TrainingLogger.Infrastructure.Strava.Models;

public record SubscriptionValidationResponse
{
    [JsonPropertyName("hub.challenge")]
    public required string Challenge { get; set; }
}
