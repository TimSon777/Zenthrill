using Microsoft.Extensions.Configuration;
using Zenthrill.Settings;

namespace Zenthrill.Outbox.Publisher;

public sealed class OutboxPublisherSettings : ISettings
{
    public static string SectionName => "OUTBOX_PUBLISHER";

    [ConfigurationKeyName("REDIS_DEDUPLICATION_PREFIX")]
    public required string RedisDeduplicationPrefix { get; set; }

    [ConfigurationKeyName("REDIS_DEDUPLICATION_EXPIRY")]
    public required TimeSpan RedisDeduplicationExpiry { get; set; }

    [ConfigurationKeyName("PARALLEL_WORKERS_COUNT")]
    public required int ParallelWorkersCount { get; set; }
    
    [ConfigurationKeyName("MESSAGE_BROKER")]
    public required RabbitMQSettings MessageBroker { get; set; }
    
    [ConfigurationKeyName("REDIS_CONNECTION_STRING")]
    public required string RedisConnectionString { get; set; }
}