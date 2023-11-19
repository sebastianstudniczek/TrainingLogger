using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class ActivitiesClient(
        IHttpClientFactory httpClientFactory, 
        IOptions<StravaOptions> options,
        ILogger<ActivitiesClient> logger) 
    : IActivitiesClient
{
    public async Task<Activity?> GetActivityByIdAsync(ulong id, CancellationToken token) {
        var httpClient = httpClientFactory.CreateClient(options.Value.HttpClientName);
        var requestPath = options.Value.GetActivityByIdUri.AppendPathSegment(id);
        var result = await httpClient.GetAsync(requestPath, token);

        if (!result.IsSuccessStatusCode) {
            logger.LogError(result.ReasonPhrase);
            return null;
        }

        string content = await result.Content.ReadAsStringAsync(token);
        var activityDto = JsonSerializer.Deserialize<ActivityDto>(content);

        return ActivityDto.MapToEntity(activityDto!);
    }
}
