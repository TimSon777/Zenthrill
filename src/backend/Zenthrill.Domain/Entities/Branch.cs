using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

public sealed class BranchId : HasId<Guid>;

public sealed class Branch : Entity<BranchId>
{
    public required string Name { get; set; }

    public required List<Fragment> Fragments { get; set; }
}