using Zenthrill.Story.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.AddOutboxMessageConsumerConfiguration();
builder.AddGraphDatabaseConfiguration();

builder.Services.AddHostedService<Consumer>();

var host = builder.Build();
host.Run();