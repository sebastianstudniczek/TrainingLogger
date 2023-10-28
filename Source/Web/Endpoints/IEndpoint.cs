namespace TrainingLogger.Web.Endpoints;

public interface IEndpoint
{
    string Pattern { get; }
    HttpMethod Method { get; }
    Delegate Handler { get; }
}
