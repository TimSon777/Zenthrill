using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Settings;
using Zenthrill.Settings.DependencyInjection;
using Zenthrill.WebAPI.BackgroundServices;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundServicesConfiguration
{
    public static IHostApplicationBuilder AddBackgroundServices(this IHostApplicationBuilder builder)
    {
        builder.AddOutboxMessageProcessorConfiguration((sp, dbContextOptionsBuilder) =>
        {
            var settings = sp.GetOptions<MainDatabaseSettings>();
            dbContextOptionsBuilder.UseNpgsql(settings.ConnectionString);
        });
        
        builder.Services.AddHostedService<OutboxPublisher>();

        return builder;
    }
}