namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static string Join(this IEnumerable<string> src, string separator)
    {
        return string.Join(separator, src);
    }

    public static (IList<T>, IList<T>) DivideBy<T>(this IEnumerable<T> src, Func<T, bool> predicate)
    {
        var trueList = new List<T>();
        var falseList = new List<T>();

        foreach (var e in src)
        {
            if (predicate(e))
            {
                trueList.Add(e);
            }
            else
            {
                falseList.Add(e);
            }
        }

        return (trueList, falseList);
    }
}