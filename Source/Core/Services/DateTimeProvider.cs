namespace TrainingLogger.Core.Services;

public delegate DateTimeOffset GetUtcNow();

public static class DateTimeProvider
{
    public static DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
}
