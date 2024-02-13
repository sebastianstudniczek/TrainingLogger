using Flurl;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TrainingLogger.Core.Contracts;
using TrainingLogger.Core.DTOs;

namespace TrainingLogger.Infrastructure.Strava.Implementations;

internal sealed class ActivitiesClient(
    IHttpClientFactory httpClientFactory, 
    IOptions<StravaOptions> options) : IActivitiesClient
{
    private readonly IOptions<StravaOptions> _options = options;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<ActivityDto?> GetActivityByIdAsync(long id, CancellationToken token) 
    {
        var httpClient = _httpClientFactory.CreateClient(Consts.StravaClientName);
        var requestPath = _options.Value.GetActivityByIdPart.AppendPathSegment(id);
        return await httpClient.GetFromJsonAsync<ActivityDto>(requestPath, token);
    }

    public async Task<IEnumerable<ActivityDto>> GetActivitiesAsync(DateTimeOffset? from = null, CancellationToken token = default)
    {
        var httpClient = _httpClientFactory.CreateClient(Consts.StravaClientName);
        var requestPath = _options.Value.GetActivitiesPart;
        return await httpClient.GetFromJsonAsync<IEnumerable<ActivityDto>>(requestPath, token) ?? Array.Empty<ActivityDto>();
    }
}
