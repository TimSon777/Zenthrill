namespace Zenthrill.Outbox.Core;

public interface IOutboxMessageHandler
{
    Task Handle(IOutboxMessage message);

    Type GetOutboxMessageType();
}