using System.Collections;

namespace DoeMais.Tests.Helpers;

public class RecordDeepEqualityComparer<T> : IEqualityComparer<T>
{
    public bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        foreach (var property in typeof(T).GetProperties())
        {
            var xValue = property.GetValue(x);
            var yValue = property.GetValue(y);

            if (xValue is IEnumerable xCollection && yValue is IEnumerable yCollection)
            {
                if (!xCollection.Cast<object>().SequenceEqual(yCollection.Cast<object>()))
                    return false;
            }
            else if (!Equals(xValue, yValue))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
}