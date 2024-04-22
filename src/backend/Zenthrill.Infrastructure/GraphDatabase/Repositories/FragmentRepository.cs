using Neo4jClient;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Repositories;
using Zenthrill.Domain.Entities;
using Zenthrill.Infrastructure.GraphDatabase.Objects;

namespace Zenthrill.Infrastructure.GraphDatabase.Repositories;

public sealed class FragmentRepository(
    BoltGraphClient boltGraphClient,
    ILabelsConverter labelsConverter) : IFragmentRepository
{
    public async Task<Fragment?> TryGetAsync(FragmentId fragmentId, StoryInfoVersionId storyInfoVersionId, CancellationToken ct)
    {
        var label = labelsConverter.Convert(storyInfoVersionId);
            
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
            Name = result.Name,
            Body = result.Body
        };
    }

    public async Task CreateAsync(Fragment fragment, StoryInfoVersionId storyInfoVersionId)
    {
        var label = labelsConverter.Convert(storyInfoVersionId);

        var fragmentId = fragment.Id.Value.ToString();

        var fragmentDto = new FragmentDto
        {
            Id = fragmentId,
            Name = fragment.Name,
            Body = fragment.Body
        };

        var fragmentParam = $"fragment{fragment.Id.Value:N}";

       await boltGraphClient.Cypher
            .Create($"({fragmentParam}:{label} ${fragmentParam})")
            .WithParam(fragmentParam, fragmentDto)
            .ExecuteWithoutResultsAsync();
    }

    public async Task UpdateAsync(Fragment fragment, StoryInfoVersionId storyInfoVersionId)
    {
        var label = labelsConverter.Convert(storyInfoVersionId);

        var fragmentId = fragment.Id.Value.ToString();

        var fragmentDto = new FragmentDto
        {
            Id = fragmentId,
            Name = fragment.Name,
            Body = fragment.Body
        };

        await boltGraphClient.Cypher
            .Match($"(fragment:{label})") // Используйте label для идентификации типа узла
            .Where("fragment.Id = $fragmentId") // Используйте параметр для идентификатора узла
            .Set("fragment = $fragmentDto") // Обновите поля узла, используя DTO
            .WithParams(new
            {
                fragmentId,
                fragmentDto
            })
            .ExecuteWithoutResultsAsync();
    }
}