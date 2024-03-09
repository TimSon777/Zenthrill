using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IFragmentRepository
{
    Task<Fragment?> TryGetAsync(FragmentId fragmentId, StoryInfoId storyInfoId, CancellationToken ct);

    Task CreateAsync(Fragment fragment, StoryInfoId storyInfoId);

    Task UpdateAsync(Fragment fragment, StoryInfoId storyInfoId);
}