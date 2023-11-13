namespace TrainingLogger.Core.Models;

public class Activity
{
    public ulong Id { get; init; }
    public required string Name { get; init; }
    public int Distance { get; init; }
    public int MovingTime { get; init; }
    public int ElapsedTime { get; init; }
    public required SportType SportType { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime StartDateLocal { get; init; }
    public double AverageCadence { get; init; }
    public required string Description { get; init; }
    public double Calories { get; init; }
}