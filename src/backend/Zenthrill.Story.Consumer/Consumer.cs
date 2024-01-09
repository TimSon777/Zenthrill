using Zenthrill.Outbox.Consumer;

namespace Zenthrill.Story.Consumer;

public sealed class Consumer(IOutboxMessageConsumer outboxMessageConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await outboxMessageConsumer.ConsumeAsync(stoppingToken);
    }
}