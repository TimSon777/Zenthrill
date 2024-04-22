using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TypesafeLocalization;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Create;

public interface IFragmentCreator
{
    Task<CreateFragmentOneOf> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken);
}

public sealed class FragmentCreator(
    ILocalizerFactory localizerFactory,
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    IValidator<CreateFragmentRequest> validator) : IFragmentCreator
{
    public async Task<CreateFragmentOneOf> CreateAsync(CreateFragmentRequest request, CancellationToken cancellationToken)
    {
        return await graphDbContext.ExecuteAsync<CreateFragmentOneOf>(async (repositoryRegistry, ct) =>
        {
            var storyInfoVersion = await applicationDbContext.StoryInfoVersions
                .Include(siv => siv.StoryInfo)
                .Include(siv => siv.SubVersions)
                .FirstOrDefaultAsync(StoryInfoVersion.ById(request.StoryInfoVersionId), cancellationToken);

            if (storyInfoVersion is null)
            {
                return NotFound.ById(request.StoryInfoVersionId);
            }

            if (!request.User.HasAccessToUpdate(storyInfoVersion.StoryInfo))
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

            var fragmentId = FragmentId.New();

            var fragment = new Fragment(fragmentId)
            {
                Name = request.Name,
                Body = request.Body
            };
            
            if (!request.FromFragmentId.HasValue)
            {
                await repositoryRegistry.FragmentRepository
                    .CreateAsync(fragment, storyInfoVersion.Id);

                return fragmentId;
            }

            var fromFragment = await repositoryRegistry.FragmentRepository
                .TryGetAsync(request.FromFragmentId.Value, request.StoryInfoVersionId, ct);

            if (fromFragment is null)
            {
                return NotFound.ById(request.FromFragmentId.Value);
            }

            var localizer = localizerFactory.CreateLocalizer(Locale.ruRU);

            var branch = new Branch(fromFragment, fragment)
            {
                Inscription = localizer.DefaultBranchName()
            };

            await repositoryRegistry.FragmentRepository
                .CreateAsync(fragment, storyInfoVersion.Id);

            await repositoryRegistry.BranchRepository
                .CreateAsync(branch, request.StoryInfoVersionId);

            return fragmentId;
        }, cancellationToken);
    }
}