namespace Zenthrill.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    public TId Id { get; set; }

    public bool Equals(Entity<TId>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Entity<TId> entity)
        {
            return entity.Equals(this);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }
}