using Microsoft.Extensions.Options;
using TrainingLogger.StravaFakeServer.Generators;

namespace TrainingLogger.StravaFakeServer;

public class ActivityPublisher : BackgroundService
{
    private readonly ILogger<ActivityPublisher> _logger;
    private readonly HttpClient _httpClient;
    private readonly TrainingLoggerOptions _webOptions;

    public ActivityPublisher(
        ILogger<ActivityPublisher> logger, 
        HttpClient httpClient,
        IOptions<TrainingLoggerOptions> webOptions)
    {
        _logger = logger;
        _httpClient = httpClient;
        _webOptions = webOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting publishing activities...");
        var activityFaker = new EventDataRequestFaker();
        _httpClient.BaseAddress = new Uri(_webOptions.BaseUri);

        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishActivity(activityFaker, stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task PublishActivity(EventDataRequestFaker eventDataFaker, CancellationToken token)
    {
        var eventData = eventDataFaker.Generate();
        _logger.LogInformation("Publishing activity event with id '{Id}'", eventData.ObjectId);
        await _httpClient.PostAsJsonAsync(_webOptions.SubscriptionEndpoint, eventData, token);
        _logger.LogInformation("Activity event with id '{Id}' published", eventData.ObjectId);
    }
}