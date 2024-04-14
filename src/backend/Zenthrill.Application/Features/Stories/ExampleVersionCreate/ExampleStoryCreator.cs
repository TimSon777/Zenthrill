using TypesafeLocalization;
using Zenthrill.Application.Outbox;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Outbox.Core;
using Zenthrill.Providers;

namespace Zenthrill.Application.Features.Stories.ExampleVersionCreate;

public interface IExampleStoryVersionCreator
{
    Task<StoryInfoVersionId> CreateAsync(ExampleStoryVersionCreateRequest request, CancellationToken cancellationToken);
}

public sealed class ExampleStoryVersionCreator(
    IApplicationDbContext applicationDbContext,
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    ILocalizerFactory localizerFactory) : IExampleStoryVersionCreator
{
    public async Task<StoryInfoVersionId> CreateAsync(ExampleStoryVersionCreateRequest request, CancellationToken cancellationToken)
    {
        var localizer = localizerFactory.CreateLocalizer(request.Locale);

        var storyInfoVersion = new StoryInfoVersion
        {
            EntrypointFragmentId = FragmentId.New(),
            Name = localizer.ExampleStoryName(),
            Version = StoryVersion.FirstVersion,
            StoryInfoId = request.StoryInfoId
        };

        applicationDbContext.StoryInfoVersions
            .Add(storyInfoVersion);

        var createExampleStoryOutboxMessage = new CreateExampleStoryOutboxMessage
        {
            StoryInfoVersionId = storyInfoVersion.Id.Value,
            Locale = request.Locale
        };

        var outboxMessage = new OutboxMessage
        {
            ScheduledAt = dateTimeOffsetProvider.UtcNow,
        };

        outboxMessage.SetMessage(createExampleStoryOutboxMessage);

        applicationDbContext.OutboxMessages
            .Add(outboxMessage);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return storyInfoVersion.Id;
    }
}