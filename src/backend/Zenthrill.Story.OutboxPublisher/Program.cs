using Microsoft.EntityFrameworkCore;
using Zenthrill.Application.Settings;
using Zenthrill.Settings.DependencyInjection;
using Zenthrill.Story.OutboxPublisher;

var builder = Host.CreateApplicationBuilder(args);

builder.AddOutboxMessageProcessorConfiguration((sp, dbContextOptionsBuilder) =>
{
    var settings = sp.GetOptions<MainDatabaseSettings>();
    dbContextOptionsBuilder.UseNpgsql(settings.ConnectionString);
});

builder.AddApplicationDbContextConfiguration();

builder.Services.AddDateTimeOffsetProvider();
builder.Services.AddHostedService<OutboxPublisher>();
builder.Services.AddOptions();

var host = builder.Build();
host.Run();