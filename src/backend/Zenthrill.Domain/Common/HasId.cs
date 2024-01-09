namespace Zenthrill.Domain.Common;

public interface IHasId;

public abstract class HasId<TId> : IHasId
    where TId : IEquatable<TId>
{
    public required TId Id { get; set; } = default!;
}