using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public interface IStoryVersionCreator
{
    Task<CreateStoryVersionOneOf> CreateAsync(CreateStoryVersionRequest request, CancellationToken cancellationToken);
}

public sealed class StoryVersionCreator(
    IApplicationDbContext applicationDbContext,
    IValidator<CreateStoryVersionRequest> validator) : IStoryVersionCreator
{
    public async Task<CreateStoryVersionOneOf> CreateAsync(CreateStoryVersionRequest request, CancellationToken cancellationToken)
    {
        var baseVersion = await applicationDbContext.StoryInfoVersions
            .Include(siv => siv.StoryInfo)
            .FirstOrDefaultAsync(StoryInfoVersion.ById(request.BaseStoryInfoVersionId), cancellationToken);
        
        if (baseVersion is null)
        {
            return NotFound.ById(request.BaseStoryInfoVersionId);
        }

        if (!request.User.HasAccessToUpdate(baseVersion.StoryInfo))
        {
            return new Forbid();
        }
        
        var validationContext = new ValidationContext<CreateStoryVersionRequest>(request)
        {
            RootContextData =
            {
                ["BaseVersions"] = baseVersion
            }
        };

        var validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ValidationFailure(validationResult.ToDictionary());
        }

        var storyInfoVersion = new StoryInfoVersion
        {
            EntrypointFragmentId = baseVersion.EntrypointFragmentId.HasValue ? FragmentId.New() : null,
            Name = request.Name,
            StoryInfo = baseVersion.StoryInfo,
            Version = StoryVersion.Create(request.Version.Major, request.Version.Minor, request.Version.Suffix).AsT0
        };

        await applicationDbContext.StoryInfoVersions.AddAsync(storyInfoVersion, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfoVersion.Id;
    }
}