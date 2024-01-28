namespace TrainingLogger.Infrastructure.Strava.Models;

public record TokenExchangeRequest(string ClientId, string ClientSecret, string RefreshToken)
{
    public string GrantType => "refresh_token";
}