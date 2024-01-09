namespace Zenthrill.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : IHasId
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