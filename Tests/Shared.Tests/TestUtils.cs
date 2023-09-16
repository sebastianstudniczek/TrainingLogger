namespace Shared.Tests;

public static class TestUtils
{
    public static int RandomInt => Guid.NewGuid().GetHashCode();
    public static string RandomString => Guid.NewGuid().ToString();
}