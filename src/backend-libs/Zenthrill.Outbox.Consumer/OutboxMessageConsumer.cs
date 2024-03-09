using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Outbox.Consumer;

public interface IOutboxMessageConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}

public sealed class OutboxMessageConsumer(
    IServiceProvider serviceProvider,
    IOptions<OutboxConsumerSettings> outboxConsumerSettingsOptions) : IOutboxMessageConsumer
{
    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var outboxConsumerSettings = outboxConsumerSettingsOptions.Value;

        var factory = new ConnectionFactory
        {
            DispatchConsumersAsync = true,
            HostName = outboxConsumerSettings.MessageBroker.Host,
            Password = outboxConsumerSettings.MessageBroker.Password,
            UserName = outboxConsumerSettings.MessageBroker.Username,
            Port = outboxConsumerSettings.MessageBroker.Port
        };

        using var connection = await factory.CreateConnectionAsync(cancellationToken);

        var tasks = new List<Task>();

        for (var i = 0; i < outboxConsumerSettings.ParallelWorkersCount; i++)
        {
            tasks.Add(ProcessMessageAsync(connection, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private async Task ProcessMessageAsync(IConnection connection, CancellationToken cancellationToken)
    {
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(
            queue: outboxConsumerSettingsOptions.Value.MessageBroker.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, @event) =>
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var handlers = scope.ServiceProvider
                    .GetRequiredService<IEnumerable<IOutboxMessageHandler>>()
                    .ToDictionary(h => h.GetOutboxMessageType(), h => h);

                foreach (var handler1 in handlers)
                {
                    Console.WriteLine(handler1.Key);
                }
                var typeNameHeader = (byte[]) @event.BasicProperties.Headers!["$type"]!;
                var typeName = Encoding.UTF8.GetString(typeNameHeader);
                var type = Type.GetType(typeName)!;
                var handler = handlers[type];
                var body = @event.Body.ToArray();
                var outboxMessage = (IOutboxMessage)JsonSerializer.Deserialize(body, type)!;
                
                await handler.Handle(outboxMessage);

                // ReSharper disable once AccessToDisposedClosure
                await channel.BasicAckAsync(
                    deliveryTag: @event.DeliveryTag,
                    multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ReSharper disable once AccessToDisposedClosure
                await channel.BasicNackAsync(
                    deliveryTag: @event.DeliveryTag,
                    multiple: false,
                    requeue: true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: outboxConsumerSettingsOptions.Value.MessageBroker.Queue,
            autoAck: false,
            consumer: consumer);

        while (!cancellationToken.IsCancellationRequested)
        {
        }
    }
}