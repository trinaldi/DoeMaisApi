using System.Reflection;

namespace DoeMais.Tests.Assertions;

public class DtoAssertions
{
    public static void AssertAreEqual<T>(T expected, T actual)
    {
        Assert.That(actual, Is.Not.Null, $"The returned object of type '{typeof(T).Name}' is null");

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var expectedValue = prop.GetValue(expected);
            var actualValue = prop.GetValue(actual);

            Assert.That(actualValue, Is.EqualTo(expectedValue),
                $"Property '{prop.Name}' does not match: expected '{expectedValue}', but got '{actualValue}'");
        }
    }
}