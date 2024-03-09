namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class ISetExtensions
{
    public static void AddRange<T>(this ISet<T> src, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            src.Add(item);
        }
    }
}