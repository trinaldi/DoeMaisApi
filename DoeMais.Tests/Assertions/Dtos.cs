using System.Collections;
using System.Reflection;

namespace DoeMais.Tests.Assertions;

public class Dtos
{
    public static void AssertAreEqual<T>(T expected, T actual)
    {
        Assert.That(actual, Is.Not.Null, $"The returned object of type '{typeof(T).Name}' is null");

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var expectedValue = prop.GetValue(expected);
            var actualValue = prop.GetValue(actual);
            var propType = prop.PropertyType;

            if (expectedValue == null && actualValue == null)
                continue;

            if (expectedValue == null || actualValue == null)
                Assert.Fail($"Property '{prop.Name}' mismatch: one is null and the other isn't.");

            if (IsSimpleType(propType))
            {
                Assert.That(actualValue, Is.EqualTo(expectedValue),
                    $"Property '{prop.Name}' mismatch: expected '{expectedValue}', got '{actualValue}'");
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
            {
                var expectedEnum = ((IEnumerable)expectedValue).Cast<object>().ToList();
                var actualEnum = ((IEnumerable)actualValue).Cast<object>().ToList();

                Assert.That(actualEnum.Count, Is.EqualTo(expectedEnum.Count),
                    $"Collection '{prop.Name}' count mismatch.");

                for (int i = 0; i < expectedEnum.Count; i++)
                {
                    AssertAreEqual(expectedEnum[i], actualEnum[i]);
                }
            }
            else
            {
                var method = typeof(Dtos).GetMethod(nameof(AssertAreEqual))!
                    .MakeGenericMethod(propType);
                method.Invoke(null, new[] { expectedValue, actualValue });
            }
        }
    }

    private static bool IsSimpleType(Type type) =>
        type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal)
        || type == typeof(Guid);
}