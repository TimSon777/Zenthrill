using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Application.Specs;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Create;

public interface IBranchCreator
{
    Task<CreateBranchOneOf> CreateAsync(
        CreateBranchRequest request,
        CancellationToken cancellationToken);
}

public sealed class BranchCreator(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    IValidator<CreateBranchRequest> validator) : IBranchCreator
{
    public async Task<CreateBranchOneOf> CreateAsync(
        CreateBranchRequest request,
        CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<CreateBranchOneOf>(
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

                var fromFragment = await repositoryRegistry.FragmentRepository
                    .TryGetAsync(request.FromFragmentId, storyInfo.Id, ct);

                var toFragment = await repositoryRegistry.FragmentRepository
                    .TryGetAsync(request.ToFragmentId, storyInfo.Id, ct);

                if (fromFragment is null || toFragment is null)
                {
                    return new FragmentDoesNotExist();
                }

                var branch = new Branch(fromFragment, toFragment)
                {
                    Inscription = request.Inscription,
                };
                
                await repositoryRegistry.BranchRepository
                    .CreateAsync(branch, storyInfo.Id);

                return branch.Id;
            },
            cancellationToken);
    }
}