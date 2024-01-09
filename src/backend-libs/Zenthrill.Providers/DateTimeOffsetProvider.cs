namespace Zenthrill.Providers;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset UtcNow { get; }
}

public sealed class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}