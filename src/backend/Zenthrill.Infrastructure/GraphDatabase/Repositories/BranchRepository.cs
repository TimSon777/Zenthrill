using Neo4jClient;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Repositories;
using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Mappers;
using Zenthrill.Infrastructure.GraphDatabase.Objects;

namespace Zenthrill.Infrastructure.GraphDatabase.Repositories;

public sealed class BranchRepository(
    BoltGraphClient boltGraphClient,
    ILabelsConverter labelsConverter) : IBranchRepository
{
    public async Task<Branch?> TryGetAsync(BranchId branchId, StoryInfoVersionId storyInfoVersionId, CancellationToken ct)
    {
        var label = labelsConverter.Convert(storyInfoVersionId);
            
        var results = await boltGraphClient.Cypher
            .Match($"(fromFragment:{label})-[branch:TRANSITION_TO]-(toFragment:{label})")
            .Where("branch.Id = $branchId")
            .WithParam("branchId", branchId.Value.ToString())
            .ReturnDistinct<BranchWithFragmentsDto>((branch, fromFragment, toFragment) => new BranchWithFragmentsDto
            {
                Branch = branch.As<BranchDto>(),
                FromFragment = fromFragment.As<FragmentDto>(),
                ToFragment = toFragment.As<FragmentDto>()
            })
            .ResultsAsync;

        return results
            .DistinctBy(r => r.Branch.Id)
            .SingleOrDefault()
            ?.MapToBranch();
    }
    
    public async Task CreateAsync(Branch branch, StoryInfoVersionId storyInfoVersionId)
    {
        var label = labelsConverter.Convert(storyInfoVersionId);

        var branchDto = new BranchDto
        {
            Id = branch.Id.Value.ToString(),
            Inscription = branch.Inscription
        };

        var fromFragmentDto = new FragmentDto
        {
            Id = branch.FromFragment.Id.Value.ToString(),
            Body = branch.FromFragment.Body
        };

        var toFragmentDto = new FragmentDto
        {
            Id = branch.ToFragment.Id.Value.ToString(),
            Body = branch.ToFragment.Body
        };

        await boltGraphClient.Cypher
            .Merge($"(fromFragment:{label} {{Id: $fromFragmentId}})")
            .OnCreate()
            .Set("fromFragment = $fromFragmentDto")
            .Merge($"(toFragment:{label} {{Id: $toFragmentId}})")
            .OnCreate()
            .Set("toFragment = $toFragmentDto")
            .Merge("(fromFragment)-[branch:TRANSITION_TO]->(toFragment)")
            .OnCreate()
            .Set("branch = $branchDto")
            .WithParam("fromFragmentId", branch.FromFragment.Id.ToString())
            .WithParam("toFragmentId", branch.ToFragment.Id.ToString())
            .WithParam("branchDto", branchDto)
            .WithParam("fromFragmentDto", fromFragmentDto)
            .WithParam("toFragmentDto", toFragmentDto)
            .ExecuteWithoutResultsAsync();
    }
    
    public async Task UpdateAsync(Branch branch, StoryInfoVersionId storyInfoVersionId)
    {
        var branchDto = new BranchDto
        {
            Id = branch.Id.Value.ToString(),
            Inscription = branch.Inscription
        };
 
        await boltGraphClient.Cypher
            .Match("(a)-[branch:TRANSITION_TO { Id: $branchId }]-(b)")
            .Set("branch = $branchDto")
            .WithParam("branchId", branch.Id.Value.ToString())
            .WithParam("branchDto", branchDto)
            .ExecuteWithoutResultsAsync();
    }
}