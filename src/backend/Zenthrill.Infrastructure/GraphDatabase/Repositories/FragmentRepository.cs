using Neo4jClient;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Repositories;
using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Objects;
using Zenthrill.Infrastructure.Repositories;

namespace Zenthrill.Infrastructure.GraphDatabase.Repositories;

public sealed class FragmentRepository(
    BoltGraphClient boltGraphClient,
    ILabelsConverter labelsConverter) : IFragmentRepository
{
    public async Task<Fragment?> TryGetAsync(FragmentId fragmentId, StoryInfoId storyInfoId, CancellationToken ct)
    {
        var label = labelsConverter.Convert(storyInfoId);
            
        var results = await boltGraphClient.Cypher
            .Match($"(fragment:{label})")
            .Where("fragment.Id = $fragmentId")
            .WithParam("fragmentId", fragmentId.Value.ToString())
            .Return<FragmentDto>("fragment")
            .ResultsAsync;

        var result = results.SingleOrDefault();

        if (result is null)
        {
            return null;
        }

        return new Fragment(new FragmentId(Guid.Parse(result.Id)))
        {
            Body = result.Body
        };
    }

    public async Task CreateAsync(Fragment fragment, StoryInfoId storyInfoId)
    {
        var label = labelsConverter.Convert(storyInfoId);

        var fragmentId = fragment.Id.Value.ToString();

        var fragmentDto = new FragmentDto
        {
            Id = fragmentId,
            Body = fragment.Body
        };

        var fragmentParam = $"fragment{fragment.Id.Value:N}";

       await boltGraphClient.Cypher
            .Create($"({fragmentParam}:{label} ${fragmentParam})")
            .WithParam(fragmentParam, fragmentDto)
            .ExecuteWithoutResultsAsync();
    }

    public async Task UpdateAsync(Fragment fragment, StoryInfoId storyInfoId)
    {
        var label = labelsConverter.Convert(storyInfoId);

        var fragmentId = fragment.Id.Value.ToString();

        var fragmentDto = new FragmentDto
        {
            Id = fragmentId,
            Body = fragment.Body
        };

        var fragmentParam = $"fragment{fragment.Id.Value:N}";
        var fragmentIdParam = $"fragmentId{fragment.Id.Value:N}";

        await boltGraphClient.Cypher
            .Match($"({fragmentParam}-{fragmentId}:{label})")
            .Where($"{fragmentParam}.Id = {fragmentIdParam}")
            .Set($"{fragmentParam} = ${fragmentParam}")
            .WithParam(fragmentIdParam, fragmentId)
            .WithParam(fragmentParam, fragmentDto)
            .ExecuteWithoutResultsAsync();
    }
}