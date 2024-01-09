using Zenthrill.Outbox.Publisher;

namespace Zenthrill.Story.OutboxPublisher;

public class OutboxPublisher(IOutboxMessagesProcessor outboxMessagesProcessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await outboxMessagesProcessor.ProcessAsync(stoppingToken);
    }
}