using System.Reflection;
using Microsoft.Extensions.Hosting;
using Zenthrill.Outbox.Consumer;
using Zenthrill.Outbox.Core;
using Zenthrill.Settings.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DiConfiguration
{
    public static IHostApplicationBuilder AddOutboxMessageConsumerConfiguration(
        this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<OutboxConsumerSettings>();

        builder.Services.AddSingleton<IOutboxMessageConsumer, OutboxMessageConsumer>();

        var handlers = Assembly.GetCallingAssembly().DefinedTypes
            .Where(x => typeof(IOutboxMessageHandler).IsAssignableFrom(x)
                && x is { IsInterface: false, IsAbstract: false });

        foreach (var handler in handlers)
        {
            var handlerType = handler.AsType();
            builder.Services.AddScoped(typeof(IOutboxMessageHandler), handlerType);
        }

        return builder;
    }
}