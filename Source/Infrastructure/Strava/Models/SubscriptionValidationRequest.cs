using Microsoft.AspNetCore.Http;

namespace TrainingLogger.Infrastructure.Strava.Models;

public record SubscriptionValidationRequest(string Mode, string Token, string Challenge)
{
    public static ValueTask<SubscriptionValidationRequest?> BindAsync(HttpContext context)
    {
        string? mode = context.Request.Query["hub.mode"];
        string? token = context.Request.Query["hub.verify_token"];
        string? challenge = context.Request.Query["hub.challenge"];

        if (string.IsNullOrEmpty(mode) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(challenge))
        {
            return ValueTask.FromResult<SubscriptionValidationRequest?>(null);
        }

        var request = new SubscriptionValidationRequest(mode, token, challenge);

        return ValueTask.FromResult<SubscriptionValidationRequest?>(request);
    }
};
