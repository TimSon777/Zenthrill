using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StackExchange.Redis;
using Zenthrill.Outbox.Core;
using Zenthrill.Providers;

namespace Zenthrill.Outbox.Publisher;

public interface IOutboxMessageBrokerProcessor
{
    Task ProcessAsync(OutboxMessage outboxMessage);
}

public sealed class OutboxMessageBrokerProcessor(
    IOptions<OutboxPublisherSettings> outboxSettingsOptions,
    IConnectionMultiplexer connectionMultiplexer) : IOutboxMessageBrokerProcessor
{
    public async Task ProcessAsync(OutboxMessage outboxMessage)
    {
        var redis = connectionMultiplexer.GetDatabase();

        var outboxSettings = outboxSettingsOptions.Value;
        
        var redisKey = $"{outboxSettings.RedisDeduplicationPrefix}{outboxMessage.Id}";

        if (await redis.KeyExistsAsync(redisKey))
        {
            return;
        }

        var factory = new ConnectionFactory
        {
            HostName = outboxSettings.MessageBroker.Host,
            Password = outboxSettings.MessageBroker.Password,
            UserName = outboxSettings.MessageBroker.Username,
            Port = outboxSettings.MessageBroker.Port
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: outboxSettings.MessageBroker.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(outboxMessage.Data);

        var properties = new BasicProperties
        {
            Headers = new Dictionary<string, object?>
            {
                { "$type", outboxMessage.Type }
            }
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: outboxSettings.MessageBroker.Queue,
            basicProperties: properties,
            body: body);

        await redis.StringSetAsync(redisKey, string.Empty, outboxSettings.RedisDeduplicationExpiry, When.Always);
    }
}