using Zenthrill.Application.Features.Entrypoint;
using Zenthrill.Application.OutboxMessages;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Story.Consumer.OutboxMessageHandlers;

public sealed class CreateStoryEntrypointOutboxMessageHandler(IEntrypointCreator entrypointCreator)
    : OutboxMessageHandlerBase<CreateStoryEntrypointOutboxMessage>
{
    protected override async Task Handle(CreateStoryEntrypointOutboxMessage message)
    {
        var request = new CreateEntrypointRequest
        {
            EntrypointFragmentId = message.FragmentId
        };

        await entrypointCreator.CreateAsync(request);
    }
}