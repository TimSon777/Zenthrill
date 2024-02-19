namespace Zenthrill.Application.Extensions;

public static class ObjectExtensions
{
    public static bool TrySetValue<T>(this object obj, string propertyName, T value)
    {
        try
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo is null)
            {
                return false;
            }

            propertyInfo.SetValue(obj, value);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static T GetValue<T>(this object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName)!;
        return (T)property.GetValue(obj)!;
    }
}