namespace Zenthrill.Domain.ValueObjects;

public static class Id
{
    public static Id<TId> Create<TId>(TId id)
        where TId : notnull
    {
        return new Id<TId>(id);
    }
}

public sealed class Id<TId>
    where TId : notnull
{
    internal Id(TId value)
    {
        Value = value;
    }

    public TId Value { get; private set; }
}
