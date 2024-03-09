using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Application.Specs;

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
                var storyInfo = await applicationDbContext.StoryInfos
                    .Include(storyInfo => storyInfo.Creator)
                    .FirstOrDefaultAsync(StoryInfoSpecs.ById(request.StoryInfoId), ct);

                if (storyInfo is null)
                {
                    return NotFound.ById(request.StoryInfoId);
                }

                var hasAccess = request.User.HasAccessToUpdate(storyInfo);

                if (!hasAccess)
                {
                    return new Forbid();
                }

                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return new ValidationFailure(result.ToDictionary());
                }

                var branch = await repositoryRegistry.BranchRepository
                    .TryGetAsync(request.BranchId, request.StoryInfoId, ct);

                if (branch is null)
                {
                    return NotFound.ById(request.BranchId);
                }

                branch.Inscription = request.Inscription;

                await repositoryRegistry.BranchRepository
                    .UpdateAsync(branch, storyInfo.Id);

                return branch.Id;
            },
            cancellationToken);
    }
}