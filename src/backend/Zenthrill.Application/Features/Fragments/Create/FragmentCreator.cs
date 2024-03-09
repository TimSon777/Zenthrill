using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Application.Specs;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Create;

public interface IFragmentCreator
{
    Task<CreateFragmentOneOf> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken);
}

public sealed class FragmentCreator(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    IValidator<CreateFragmentRequest> validator) : IFragmentCreator
{
    public async Task<CreateFragmentOneOf> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<CreateFragmentOneOf>(async (repositoryRegistry, ct) =>
        {
            var storyInfo = await applicationDbContext.StoryInfos
                .Include(storyInfo => storyInfo.Creator)
                .FirstOrDefaultAsync(StoryInfoSpecs.ById(request.StoryInfoId), cancellationToken);

            if (storyInfo is null)
            {
                return NotFound.ById(request.StoryInfoId);
            }

            if (!request.User.HasAccessToUpdate(storyInfo))
            {
                return new Forbid();
            }

            var result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return new ValidationFailure(result.ToDictionary());
            }

            var fragmentId = FragmentId.New();

            var fragment = new Fragment(fragmentId)
            {
                Body = request.Body
            };

            await repositoryRegistry.FragmentRepository
                .CreateAsync(fragment, storyInfo.Id);

            return fragmentId;
        }, cancellationToken);
    }
}