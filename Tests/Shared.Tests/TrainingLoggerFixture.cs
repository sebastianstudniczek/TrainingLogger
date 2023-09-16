using AutoFixture;

namespace Shared.Tests;

public class TrainingLoggerFixture : Fixture
{
    public TrainingLoggerFixture()
    {
        Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
