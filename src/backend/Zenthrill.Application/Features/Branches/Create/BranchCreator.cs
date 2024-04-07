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

                var fromFragment = await repositoryRegistry.FragmentRepository
                    .TryGetAsync(request.FromFragmentId, storyInfoVersion.Id, ct);

                var toFragment = await repositoryRegistry.FragmentRepository
                    .TryGetAsync(request.ToFragmentId, storyInfoVersion.Id, ct);

                if (fromFragment is null || toFragment is null)
                {
                    return new FragmentDoesNotExist();
                }

                var branch = new Branch(fromFragment, toFragment)
                {
                    Inscription = request.Inscription,
                };
                
                await repositoryRegistry.BranchRepository
                    .CreateAsync(branch, storyInfoVersion.Id);

                return branch.Id;
            },
            cancellationToken);
    }
}