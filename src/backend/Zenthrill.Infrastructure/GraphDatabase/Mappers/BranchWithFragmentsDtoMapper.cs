using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Objects;

namespace Zenthrill.Infrastructure.GraphDatabase.Mappers;

public static class BranchWithFragmentsDtoMapper
{
    public static Branch MapToBranch(this BranchWithFragmentsDto branchWithFragmentsDto)
    {
        var fromFragment = branchWithFragmentsDto.FromFragment.MapToFragment();
        var toFragment = branchWithFragmentsDto.ToFragment.MapToFragment();

        return new Branch(fromFragment, toFragment)
        {
            Id = new BranchId(Guid.Parse(branchWithFragmentsDto.Branch.Id)),
            Inscription = branchWithFragmentsDto.Branch.Inscription
        };
    }
}