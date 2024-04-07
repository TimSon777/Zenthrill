using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Update;

public interface IBranchUpdater
{
    Task<UpdateBranchOneOf> UpdateAsync(
        UpdateBranchRequest request,
        CancellationToken cancellationToken);
}

public sealed class BranchUpdater(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    IValidator<UpdateBranchRequest> validator) : IBranchUpdater
{
    public async Task<UpdateBranchOneOf> UpdateAsync(
        UpdateBranchRequest request,
        CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<UpdateBranchOneOf>(
            async (repositoryRegistry, ct) =>
            {
                var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                    .Include(siv => siv.StoryInfo)
                    .Include(siv => siv.SubVersions)
                    .FirstOrDefaultAsync(StoryInfoVersion.ById(request.StoryInfoVersionId), ct);

                if (storyInfoVersion is null)
                {
                    return NotFound.ById(request.StoryInfoVersionId);
                }

                var hasAccess = request.User.HasAccessToUpdate(storyInfoVersion.StoryInfo);

                if (!hasAccess)
                {
                    return new Forbid();
                }

                if (storyInfoVersion.IsBaseVersion)
                {
                    return new ForbidEditBaseVersion();
                }

                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return new ValidationFailure(result.ToDictionary());
                }

                var branch = await repositoryRegistry.BranchRepository
                    .TryGetAsync(request.BranchId, request.StoryInfoVersionId, ct);

                if (branch is null)
                {
                    return NotFound.ById(request.BranchId);
                }

                branch.Inscription = request.Inscription;

                await repositoryRegistry.BranchRepository
                    .UpdateAsync(branch, storyInfoVersion.Id);

                return branch.Id;
            },
            cancellationToken);
    }
}