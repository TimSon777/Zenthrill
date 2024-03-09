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
            var storyInfo = await applicationDbContext.StoryInfos
                .Include(si => si.Creator)
                .FirstOrDefaultAsync(StoryInfo.ById(request.StoryInfoId), cancellationToken);

            if (storyInfo is null)
            {
                return NotFound.ById(request.StoryInfoId);
            }

            if (!request.User.HasAccessToUpdate(storyInfo))
            {
                return new Forbid();
            }

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ValidationFailure(validationResult.ToDictionary());
            }

            var fragment = await repositoryRegistry.FragmentRepository
                .TryGetAsync(request.FragmentId, storyInfo.Id, ct);

            if (fragment is null)
            {
                return NotFound.ById(request.FragmentId);
            }

            fragment.Body = request.Body;

            await repositoryRegistry.FragmentRepository
                .UpdateAsync(fragment, storyInfo.Id);

            return new Success();
        }, cancellationToken);
    }
}