using TypesafeLocalization;
using Zenthrill.Application.Features.Stories.ExampleCreate;
using Zenthrill.Domain.Entities;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Application.Outbox;

public sealed class CreateExampleStoryOutboxMessage : IOutboxMessage
{
    public required Guid StoryInfoId { get; set; }

    public required Locale Locale { get; set; }
}

public sealed class CreateExampleStoryOutboxMessageHandler(IExampleStoryCreatorCallback exampleStoryCreatorCallback)
    : OutboxMessageHandlerBase<CreateExampleStoryOutboxMessage>
{
    protected override async Task Handle(CreateExampleStoryOutboxMessage message)
    {
        var request = new ExampleStoryCreateCallbackRequest
        {
            Locale = message.Locale,
            StoryInfoId = new StoryInfoId(message.StoryInfoId)
        };

        await exampleStoryCreatorCallback.CreateCallbackAsync(request, CancellationToken.None);
    }
}