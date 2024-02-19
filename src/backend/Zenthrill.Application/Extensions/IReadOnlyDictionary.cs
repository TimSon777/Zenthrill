namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class IReadOnlyDictionary
{
    public static T ToObject<T>(this IReadOnlyDictionary<string, object?> src)
        where T : notnull
    {
        var obj = Activator.CreateInstance<T>();

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!src.TryGetValue(property.Name, out var value))
            {
                continue;
            }

            if (value == null || !property.CanWrite)
            {
                continue;
            }

            if (value is string str && Guid.TryParse(str, out var guid))
            {
                property.SetValue(obj, guid);
            }
            else
            {
                property.SetValue(obj, value);
            }
        }

        return obj;
    }
}