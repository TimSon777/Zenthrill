using Zenthrill.Outbox.Publisher;

namespace Zenthrill.WebAPI.BackgroundServices;

public class OutboxPublisher(IOutboxMessagesProcessor outboxMessagesProcessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await outboxMessagesProcessor.ProcessAsync(stoppingToken);
    }
}