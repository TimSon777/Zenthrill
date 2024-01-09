namespace Zenthrill.Domain.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string src)
    {
        return string.IsNullOrWhiteSpace(src);
    }
}