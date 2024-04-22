using Zenthrill.Application.Features.Stories.CreateVersion;
using Zenthrill.Domain.Entities;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Application.Outbox;

public sealed class CreateVersionOutboxMessage : IOutboxMessage
{
    public required Guid StoryInfoVersionId { get; set; }
}

public sealed class CreateVersionOutboxMessageHandler(IStoryVersionCreatorCallback storyVersionCreatorCallback)
    : OutboxMessageHandlerBase<CreateVersionOutboxMessage>
{
    protected override async Task Handle(CreateVersionOutboxMessage message)
    {
        var request = new CreateStoryVersionCallbackRequest
        {
            StoryInfoVersionId = new StoryInfoVersionId(message.StoryInfoVersionId)
        };

        await storyVersionCreatorCallback.CreateCallbackAsync(request, CancellationToken.None);
    }
}