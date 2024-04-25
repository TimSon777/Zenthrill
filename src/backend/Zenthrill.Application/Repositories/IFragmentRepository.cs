using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IFragmentRepository
{
    Task<Fragment?> TryGetAsync(FragmentId fragmentId, CancellationToken ct);

    Task<Fragment?> TryGetAsync(FragmentId fragmentId, StoryInfoVersionId storyInfoVersionId, CancellationToken ct);

    Task CreateAsync(Fragment fragment, StoryInfoVersionId storyInfoVersionId);

    Task UpdateAsync(Fragment fragment, StoryInfoVersionId storyInfoVersionId);
}