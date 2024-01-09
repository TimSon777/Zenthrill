namespace Zenthrill.Outbox.Core;

public abstract class OutboxMessageHandlerBase<TOutboxMessage> : IOutboxMessageHandler
    where TOutboxMessage : IOutboxMessage
{
    public async Task Handle(IOutboxMessage message)
    {
        if (message is TOutboxMessage outboxMessage)
        {
            await Handle(outboxMessage);
            return;
        }

        var inputMessageName = message.GetType().Name;
        var expectedMessageName = typeof(TOutboxMessage).Name;

        throw new InvalidOperationException($"Outbox message of type {inputMessageName} doesn't equal {expectedMessageName}");
    }

    public Type GetOutboxMessageType()
    {
        return typeof(TOutboxMessage);
    }

    protected abstract Task Handle(TOutboxMessage message);
}