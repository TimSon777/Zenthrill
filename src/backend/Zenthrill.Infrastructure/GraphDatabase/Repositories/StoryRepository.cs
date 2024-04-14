using Neo4jClient;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Repositories;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Objects;
using Zenthrill.Infrastructure.Repositories;

namespace Zenthrill.Infrastructure.GraphDatabase.Repositories;

public sealed class StoryRepository(
    BoltGraphClient boltGraphClient,
    ILabelsConverter labelsConverter) : IStoryRepository
{
    public async Task<StoryVersion> ReadAsync(StoryInfoVersion storyInfoVersion, CancellationToken cancellationToken)
    {
        var label = labelsConverter.Convert(storyInfoVersion.Id);
        
        var fragmentResults = await boltGraphClient.Cypher
            .Match($"(fragment:{label})")
            .ReturnDistinct<FragmentDto>("fragment")
            .ResultsAsync;

        var fragments = fragmentResults
            .Select(fragmentResult =>
            {
                var fragmentId = new FragmentId(Guid.Parse(fragmentResult.Id));
                return new Fragment
                {
                    Body = fragmentResult.Body,
                    Id = fragmentId,
                    IsEntrypoint = storyInfoVersion.EntrypointFragmentId == fragmentId
                };
            })
            .ToList();

        var branchesResults = await boltGraphClient.Cypher
            .Match($"(fromFragment:{label})-[branch:TRANSITION_TO]->(toFragment:{label})")
            .ReturnDistinct<BranchWithFragmentsDto>((branch, fromFragment, toFragment) => new BranchWithFragmentsDto
            {
                Branch = branch.As<BranchDto>(),
                FromFragment = fromFragment.As<FragmentDto>(),
                ToFragment = toFragment.As<FragmentDto>()
            })
            .ResultsAsync;

        foreach (var branchResult in branchesResults)
        {
            var fromFragment = fragments.First(f => f.Id.Value == Guid.Parse(branchResult.FromFragment.Id));
            var toFragment = fragments.First(f => f.Id.Value == Guid.Parse(branchResult.ToFragment.Id));

            _ = new Branch(fromFragment, toFragment)
            {
                Inscription = branchResult.Branch.Inscription,
                Id = new BranchId(Guid.Parse(branchResult.Branch.Id))
            };
        }

        var story = new StoryVersion { StoryInfoVersion = storyInfoVersion };

        foreach (var fragment in fragments)
        {
            story.AddComponent(fragment);
        }

        return story;
    }
}