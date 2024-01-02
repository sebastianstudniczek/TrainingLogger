using Microsoft.Extensions.Configuration;

namespace TrainingLogger.Shared;

public static class OptionsExtensions
{
    public static TOptions BindOptions<TOptions>(this IConfigurationSection section)
        where TOptions : new()
    {
        var options = new TOptions();
        section.Bind(options);

        return options;
    }
}
