using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IBranchRepository
{
    Task<Branch?> TryGetAsync(BranchId branchId, StoryInfoVersionId storyInfoVersionId, CancellationToken cancellationToken);

    Task CreateAsync(Branch branch, StoryInfoVersionId storyInfoVersionId);

    Task UpdateAsync(Branch branch, StoryInfoVersionId storyInfoVersionId);
}