
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace TrainingLogger.Infrastructure.Strava;

public sealed class StravaOptions
{
    public const string Strava = "Strava";

    [Required]
    public string AuthScheme { get; set; } = default!;
    [Required]
    public string HttpClientName { get; set; } = default!;
    [Required]
    public int ClientId { get; set; }
    [Required]
    public string ClientSecret { get; set; } = default!;
    [Required]
    public string AuthorizationCode { get; set; } = default!;
    [Url] 
    public string BaseUri { get; set; } = default!;
    [Required]
    public string GetTokenUri { get; set; } = default!;
    [Required]
    public string GetActivitiesUri { get; set; } = default!;
    [Required]
    public string GetActivityByIdUri { get; set; } = default!;
    [Required]
    public string WebhookVerifyToken { get; set; } = default!;
}