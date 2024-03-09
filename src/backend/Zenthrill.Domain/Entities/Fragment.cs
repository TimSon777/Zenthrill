using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct FragmentId;

public sealed class Fragment : Entity<FragmentId>
{
    public required string Body { get; set; }

    public List<Branch> InputBranches { get; set; } = [];

    public List<Branch> OutputBranches { get; set; } = [];

    public List<Branch> Branches => InputBranches.Union(OutputBranches).ToList();

    public bool IsEntrypoint { get; set; }

    public Fragment()
    {
        Id = FragmentId.New();
    }

    public Fragment(FragmentId fragmentId)
    {
        Id = fragmentId;
    }

    public HashSet<Fragment> TraverseFragments()
    {
        var hashSet = new HashSet<Fragment>();
        TraverseFragments(hashSet);
        return hashSet;
    }

    public HashSet<Branch> TraverseBranches()
    {
        var hashSet = new HashSet<Branch>();
        TraverseBranches(hashSet);
        return hashSet;
    }

    private void TraverseBranches(ISet<Branch> branches)
    {
        foreach (var fragment in Branches
            .Where(branches.Add)
            .SelectMany(branch => new[] { branch.FromFragment, branch.ToFragment }))
        {
            fragment.TraverseBranches(branches);
        }
    }

    private void TraverseFragments(ISet<Fragment> fragments)
    {
        if (!fragments.Add(this))
        {
            return;
        }

        foreach (var subFragment in Branches.SelectMany(branch => new[] { branch.FromFragment, branch.ToFragment }))
        {
            subFragment.TraverseFragments(fragments);
        }
    }
}
