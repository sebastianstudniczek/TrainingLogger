using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class ActivitiesClient(
    IHttpClientFactory httpClientFactory, 
    IOptions<StravaOptions> options, 
    ILogger<ActivitiesClient> logger) : IActivitiesClient
{
    private readonly IOptions<StravaOptions> _options = options;
    private readonly ILogger<ActivitiesClient> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<ActivityDto?> GetActivityByIdAsync(long id, CancellationToken token) {
        var httpClient = _httpClientFactory.CreateClient(Consts.StravaClientName);
        var requestPath = _options.Value.GetActivityByIdPart.AppendPathSegment(id);
        var result = await httpClient.GetAsync(requestPath, token);

        if (!result.IsSuccessStatusCode) {
            _logger.LogError(result.ReasonPhrase);
            return null;
        }

        string content = await result.Content.ReadAsStringAsync(token);
        var activityDto = JsonSerializer.Deserialize<ActivityDto>(content);

        return activityDto;
    }
}
