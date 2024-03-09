using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct BranchId;

public sealed class Branch : Entity<BranchId>
{
    public required string Inscription { get; set; }

    public readonly Fragment FromFragment;

    public readonly Fragment ToFragment;

    public Branch(Fragment fromFragment, Fragment toFragment)
    {
        Id = BranchId.New();

        toFragment.InputBranches.Add(this);
        fromFragment.OutputBranches.Add(this);

        FromFragment = fromFragment;
        ToFragment = toFragment;
    }
}