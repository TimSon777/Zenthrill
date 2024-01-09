using System.Text.Json;

namespace Zenthrill.Outbox.Core;

public sealed class OutboxMessage
{
    public OutboxMessage()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public required DateTimeOffset ScheduledAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }

    public string Data { get; set; } = default!;

    public string Type { get; set; } = default!;

    public void SetMessage<TOutboxMessage>(TOutboxMessage message)
    {
        Type = typeof(TOutboxMessage).AssemblyQualifiedName!;
        Data = JsonSerializer.Serialize(message);
    }
}