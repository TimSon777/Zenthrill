using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OneOf.Types;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using NotFound = Zenthrill.Application.Results.NotFound;

namespace Zenthrill.Application.Features.Fragments.Update;

public interface IFragmentUpdater
{
    Task<UpdateFragmentOneOf> UpdateAsync(UpdateFragmentRequest request, CancellationToken cancellationToken);
}

public sealed class FragmentUpdater(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    IValidator<UpdateFragmentRequest> validator) : IFragmentUpdater
{
    public async Task<UpdateFragmentOneOf> UpdateAsync(UpdateFragmentRequest request, CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<UpdateFragmentOneOf>(async (repositoryRegistry, ct) =>
        {
            var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                .Include(siv => siv.StoryInfo)
                .Include(siv => siv.SubVersions)
                .FirstOrDefaultAsync(StoryInfoVersion.ById(request.StoryInfoVersionId), cancellationToken);

            if (storyInfoVersion is null)
            {
                return NotFound.ById(request.StoryInfoVersionId);
            }
            
            if (storyInfoVersion.IsBaseVersion)
            {
                return new ForbidEditBaseVersion();
            }
 
            if (!request.User.HasAccessToUpdate(storyInfoVersion.StoryInfo))
            {
                return new Forbid();
            }

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ValidationFailure(validationResult.ToDictionary());
            }

            var fragment = await repositoryRegistry.FragmentRepository
                .TryGetAsync(request.FragmentId, storyInfoVersion.Id, ct);

            if (fragment is null)
            {
                return NotFound.ById(request.FragmentId);
            }

            fragment.Body = request.Body;

            await repositoryRegistry.FragmentRepository
                .UpdateAsync(fragment, storyInfoVersion.Id);

            return new Success();
        }, cancellationToken);
    }
}