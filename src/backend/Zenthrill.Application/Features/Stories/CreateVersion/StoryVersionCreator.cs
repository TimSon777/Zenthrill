using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Outbox;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Outbox.Core;
using Zenthrill.Providers;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public interface IStoryVersionCreator
{
    Task<CreateStoryVersionOneOf> CreateAsync(CreateStoryVersionRequest request, CancellationToken cancellationToken);
}

public sealed class StoryVersionCreator(
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    IApplicationDbContext applicationDbContext,
    IValidator<CreateStoryVersionRequest> validator) : IStoryVersionCreator
{
    public async Task<CreateStoryVersionOneOf> CreateAsync(CreateStoryVersionRequest request, CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .Where(StoryInfo.ById(request.StoryInfoId))
            .Include(si => si.Versions
                .Where(siv => siv.Id == request.BaseStoryInfoVersionId))
            .FirstOrDefaultAsync(cancellationToken);

        if (storyInfo is null)
        {
            return NotFound.ById(request.StoryInfoId);
        }

        StoryInfoVersion? baseVersion = null;
        if (request.BaseStoryInfoVersionId is not null)
        {
            baseVersion = await applicationDbContext.StoryInfoVersions
                .Include(siv => siv.StoryInfo)
                .FirstOrDefaultAsync(StoryInfoVersion.ById(request.BaseStoryInfoVersionId.Value), cancellationToken);

            if (baseVersion is null)
            {
                return NotFound.ById(request.BaseStoryInfoVersionId.Value);
            }
        }

        if (!request.User.HasAccessToUpdate(storyInfo))
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
            EntrypointFragmentId = baseVersion?.EntrypointFragmentId.HasValue == true ? FragmentId.New() : null,
            Name = request.Name,
            StoryInfo = storyInfo,
            Version = StoryVersion.Create(request.Version.Major, request.Version.Minor, request.Version.Suffix).AsT0,
            BaseVersion = baseVersion
        };

        if (baseVersion is not null)
        {
            var createExampleStoryOutboxMessage = new CreateVersionOutboxMessage
            {
                StoryInfoVersionId = storyInfoVersion.Id.Value
            };

            var outboxMessage = new OutboxMessage
            {
                ScheduledAt = dateTimeOffsetProvider.UtcNow
            };

            outboxMessage.SetMessage(createExampleStoryOutboxMessage);

            await applicationDbContext.OutboxMessages
                .AddAsync(outboxMessage, cancellationToken);
        }

        await applicationDbContext.StoryInfoVersions.AddAsync(storyInfoVersion, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfoVersion.Id;
    }
}