namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class ICollectionExtensions
{
    public static ICollection<T> AddIf<T>(this ICollection<T> src, T item, bool condition)
    {
        if (condition)
        {
            src.Add(item);
        }

        return src;
    }
}