namespace Zenthrill.Application.Results;

public static class NotFound
{
    public static NotFound<TId> ById<TId>(TId id)
    {
        return new NotFound<TId>
        {
            Id = id
        };
    }
}

public sealed class NotFound<TId>
{
    public required TId Id { get; init; }

    internal NotFound()
    {
    }
}