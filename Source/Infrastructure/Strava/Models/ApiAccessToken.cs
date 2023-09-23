namespace TrainingLogger.Infrastructure.Strava.Models;

internal class ApiAccessToken
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public int ExpiresAt { get; set; }
    public int ExpiresIn { get; set; }
}
