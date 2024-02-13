namespace TrainingLogger.Shared.Tests;
public static class UriExtensions
{
    private const char _ParamSeparator = '&';
    private const char _ValueSeparator = '=';

    public static string? GetQueryParamValue(this Uri uri, string paramName) => uri
        .Query[1..]
        .Split(_ParamSeparator)
        .Select(param =>
        {
            var splitted = param.Split(_ValueSeparator);
            return new
            {
                Key = splitted[0],
                Value = splitted[1]
            };
        })
        .FirstOrDefault(x => x.Key == paramName)
        ?.Value;
}
