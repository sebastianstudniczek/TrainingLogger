using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class ActivitiesClient : IActivitiesClient
{
    private readonly IOptions<StravaOptions> _options;
    private readonly ILogger<ActivitiesClient> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ActivitiesClient(IHttpClientFactory httpClientFactory, IOptions<StravaOptions> options, ILogger<ActivitiesClient> logger)
    {
        _options = options;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Activity?> GetActivityByIdAsync(ulong id, CancellationToken token) {
        var httpClient = _httpClientFactory.CreateClient(_options.Value.HttpClientName);
        var requestPath = _options.Value.GetActivityByIdUri.AppendPathSegment(id);
        var result = await httpClient.GetAsync(requestPath, token);

        if (!result.IsSuccessStatusCode) {
            _logger.LogError(result.ReasonPhrase);
            return null;
        }

        string content = await result.Content.ReadAsStringAsync(token);
        var activityDto = JsonSerializer.Deserialize<ActivityDto>(content);

        return ActivityDto.MapToEntity(activityDto!);
    }
}
