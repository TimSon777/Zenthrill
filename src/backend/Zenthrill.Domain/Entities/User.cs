using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

public sealed class UserId : HasId<Guid>;

public sealed class User : Entity<UserId>
{
    public required string Nickname { get; set; }
}