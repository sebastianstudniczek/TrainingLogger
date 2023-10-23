namespace TrainingLogger.Infrastructure.Strava.Interfaces;

internal interface ITokenContext
{
    Task<TResult> RunAsync<TResult>(
        Func<HttpClient, Task<TResult>> requestToExecute,
        GetRefreshedToken fetchNewToken,
        CancellationToken cancellationToken);
}
