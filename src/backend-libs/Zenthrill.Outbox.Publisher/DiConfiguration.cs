using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Zenthrill.Outbox.EntityFrameworkCore;
using Zenthrill.Outbox.Publisher;
using Zenthrill.Settings.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DiConfiguration
{
    public static IHostApplicationBuilder AddOutboxMessageProcessorConfiguration(
        this IHostApplicationBuilder builder,
        Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsBuilder)
    {
        builder.ConfigureSettings<OutboxPublisherSettings>();

        builder.Services.AddSingleton<IOutboxMessageBrokerProcessor, OutboxMessageBrokerProcessor>();
        builder.Services.AddSingleton<IOutboxMessageProcessor, OutboxMessageProcessor>();
        builder.Services.AddSingleton<IOutboxMessagesProcessor, OutboxMessagesProcessor>();

        builder.Services.AddDbContextFactory<OutboxDbContext>(dbContextOptionsBuilder);

        builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetOptions<OutboxPublisherSettings>();
            return ConnectionMultiplexer.Connect(settings.RedisConnectionString);
        });
        
        return builder;
    }
}