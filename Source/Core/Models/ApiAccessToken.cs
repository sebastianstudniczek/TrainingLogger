namespace TrainingLogger.Core.Models;
// TODO: Should it be here? (Infrastructure)
public class ApiAccessToken
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public int ExpiresAt { get; set; }
    public int ExpiresIn { get; set; }
}
