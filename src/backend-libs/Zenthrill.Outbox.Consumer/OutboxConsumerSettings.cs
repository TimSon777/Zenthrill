using Microsoft.Extensions.Configuration;
using Zenthrill.Settings;

namespace Zenthrill.Outbox.Consumer;

public sealed class OutboxConsumerSettings : ISettings
{
    public static string SectionName => "OUTBOX_CONSUMER";
    
    [ConfigurationKeyName("PARALLEL_WORKERS_COUNT")]
    public required int ParallelWorkersCount { get; set; }

    [ConfigurationKeyName("MESSAGE_BROKER")]
    public required RabbitMQSettings MessageBroker { get; set; }

}