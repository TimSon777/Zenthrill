using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Read;

public interface IFragmentReader
{
    Task<ReadFragmentOneOf> ReadAsync(ReadFragmentRequest request, CancellationToken cancellationToken);
}

public sealed class FragmentReader(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IFragmentReader
{
    public async Task<ReadFragmentOneOf> ReadAsync(ReadFragmentRequest request, CancellationToken cancellationToken)
    {
        return await graphDbContext.ReadAsync<ReadFragmentOneOf>(async (registry, ct) =>
        {
            var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                .FirstOrDefaultAsync(StoryInfoVersion.ById(request.StoryInfoVersionId), ct);

            if (storyInfoVersion is null)
            {
                return NotFound.ById(request.StoryInfoVersionId);
            }
            
            var story = await registry.StoryRepository.ReadAsync(storyInfoVersion, ct);

            var fragment = story.Components.SelectMany(c => c.TraverseFragments())
                .FirstOrDefault(f => f.Id == request.Id);

            if (fragment is null)
            {
                return NotFound.ById(request.Id);
            }

            return fragment;
        }, cancellationToken);
    }
}