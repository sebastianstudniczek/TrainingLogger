namespace Shared.Exceptions;

public abstract class TrainingLoggerException : Exception
{
    protected TrainingLoggerException(string message) : base(message)
    {
        
    }
}
