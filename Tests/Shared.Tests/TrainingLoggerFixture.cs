using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace TrainingLogger.Shared.Tests;

public class TrainingLoggerFixture : Fixture
{
    public TrainingLoggerFixture()
    {
        Behaviors.Add(new OmitOnRecursionBehavior());
        Customize(new AutoNSubstituteCustomization());
    }
}
