using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingLogger.Infrastructure.EF;

namespace TrainingLogger.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        var connectionString = configuration.GetConnectionString("Sqlite");

        if (connectionString is null)
        {
            throw new ArgumentNullException("connectionString cannot be null");
        }

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}
