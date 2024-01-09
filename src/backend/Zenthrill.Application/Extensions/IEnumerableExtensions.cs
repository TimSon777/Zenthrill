namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static string Join(this IEnumerable<string> src, string separator)
    {
        return string.Join(separator, src);
    }
}