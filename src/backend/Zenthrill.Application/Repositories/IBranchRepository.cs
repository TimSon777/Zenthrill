using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IBranchRepository
{
    Task<Branch?> TryGetAsync(BranchId branchId, StoryInfoId storyInfoId, CancellationToken cancellationToken);

    Task CreateAsync(Branch branch, StoryInfoId storyInfoId);

    Task UpdateAsync(Branch branch, StoryInfoId storyInfoId);
}