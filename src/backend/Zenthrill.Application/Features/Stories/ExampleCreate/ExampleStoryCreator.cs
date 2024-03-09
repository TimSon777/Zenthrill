using TypesafeLocalization;
using Zenthrill.Application.Outbox;
using Zenthrill.Domain.Entities;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Outbox.Core;
using Zenthrill.Providers;

namespace Zenthrill.Application.Features.Stories.ExampleCreate;

public interface IExampleStoryCreator
{
    Task<StoryInfoId> CreateAsync(ExampleStoryCreateRequest request, CancellationToken cancellationToken);
}

public sealed class ExampleStoryCreator(
    IApplicationDbContext applicationDbContext,
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    ILocalizerFactory localizerFactory) : IExampleStoryCreator
{
    public async Task<StoryInfoId> CreateAsync(ExampleStoryCreateRequest request, CancellationToken cancellationToken)
    {
        var localizer = localizerFactory.CreateLocalizer(request.Locale);

        applicationDbContext.Users.Attach(request.User);

        var storyInfo = new StoryInfo
        {
            Creator = request.User,
            EntrypointFragmentId = FragmentId.New(),
            StoryName = localizer.ExampleStoryName(),
            Version = StoryVersion.FirstVersion
        };

        applicationDbContext.StoryInfos
            .Add(storyInfo);

        var createExampleStoryOutboxMessage = new CreateExampleStoryOutboxMessage
        {
            StoryInfoId = storyInfo.Id.Value,
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

        return storyInfo.Id;
    }
}