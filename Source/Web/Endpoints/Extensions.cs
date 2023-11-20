using System.Reflection;

namespace TrainingLogger.Web.Endpoints;

public static class Extensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        var endpoints = Assembly
            .GetExecutingAssembly()
            .DefinedTypes
            .Where(x => typeof(IEndpoint).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            builder.MapEndpoint(endpoint);
        }

        return builder;
    }

    private static IEndpointRouteBuilder MapEndpoint(this IEndpointRouteBuilder builder, IEndpoint endpoint)
    {
        builder.MapMethods(
            endpoint.Pattern,
            new[] { endpoint.Method.ToString() },
            endpoint.Handler);

        return builder;
    }
}
