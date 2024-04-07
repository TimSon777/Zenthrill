using TypesafeLocalization;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;
using Zenthrill.Domain.Entities;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Application.Outbox;

public sealed class CreateExampleStoryOutboxMessage : IOutboxMessage
{
    public required Guid StoryInfoVersionId { get; set; }

    public required Locale Locale { get; set; }
}

public sealed class CreateExampleStoryOutboxMessageHandler(IExampleStoryCreatorCallback exampleStoryCreatorCallback)
    : OutboxMessageHandlerBase<CreateExampleStoryOutboxMessage>
{
    protected override async Task Handle(CreateExampleStoryOutboxMessage message)
    {
        var request = new ExampleStoryVersionCreateCallbackRequest
        {
            Locale = message.Locale,
            StoryInfoVersionId = new StoryInfoVersionId(message.StoryInfoVersionId)
        };

        await exampleStoryCreatorCallback.CreateCallbackAsync(request, CancellationToken.None);
    }
}