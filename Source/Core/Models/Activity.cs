namespace TrainingLogger.Core.Models;

public class Activity
{
    public ulong Id { get; set; }
    public required string Name { get; set; }
    public int Distance { get; set; }
    public int MovingTime { get; set; }
    public int ElapsedTime { get; set; }
    public required SportType SportType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StartDateLocal { get; set; }
    public required string Description { get; set; }
    public double Calories { get; set; }
}