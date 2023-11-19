using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Shared.Tests;

public class TrainingLoggerFixture : Fixture
{
    public TrainingLoggerFixture()
    {
        Behaviors.Add(new OmitOnRecursionBehavior());
        Customize(new AutoNSubstituteCustomization());
    }
}
