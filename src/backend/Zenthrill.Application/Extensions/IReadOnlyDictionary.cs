namespace Zenthrill.Application.Extensions;

// ReSharper disable once InconsistentNaming
public static class IReadOnlyDictionary
{
    public static T ToObject<T>(this IReadOnlyDictionary<string, object?> src)
    {
        var obj = Activator.CreateInstance<T>();

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!src.TryGetValue(property.Name, out var value))
            {
                continue;
            }
            
            if (value != null && property.CanWrite && property.PropertyType.IsInstanceOfType(value))
            {
                property.SetValue(obj, value);
            }
        }

        return obj;
    }
}