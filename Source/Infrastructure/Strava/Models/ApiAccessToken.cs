namespace TrainingLogger.Infrastructure.Strava.Models;

public class ApiAccessToken
{
    public Guid Id { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public int ExpiresAt { get; set; }
    public int ExpiresIn { get; set; }
}
