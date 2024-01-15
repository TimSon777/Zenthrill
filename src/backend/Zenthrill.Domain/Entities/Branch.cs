using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct BranchId;

public sealed class Branch : Entity<BranchId>
{
    public required string Inscription { get; set; }

    public required List<Fragment> Fragments { get; set; }

    public Branch()
    {
        Id = BranchId.New();
    }
}