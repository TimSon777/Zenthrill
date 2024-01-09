using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Zenthrill.Outbox.Publisher;

public interface IOutboxMessagesProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

public sealed class OutboxMessagesProcessor(
    IServiceProvider serviceProvider,
    IOptions<OutboxPublisherSettings> outboxSettingsOptions) : IOutboxMessagesProcessor
{
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var outboxSettings = outboxSettingsOptions.Value;
        var tasks = new List<Task>();

        for (var i = 0; i < outboxSettings.ParallelWorkersCount; i++)
        {
            tasks.Add(RunProcessorAsync(cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private async Task RunProcessorAsync(CancellationToken cancellationToken)
    {
        var messageProcessor = serviceProvider.GetRequiredService<IOutboxMessageProcessor>();
        await messageProcessor.ProcessAsync(cancellationToken);
    }
}