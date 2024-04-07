using Zenthrill.Application.Outbox;
using Zenthrill.Story.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.AddOutboxMessageConsumerConfiguration(typeof(CreateExampleStoryOutboxMessage).Assembly);
builder.AddGraphDatabaseConfiguration();
builder.AddApplicationDbContextConfiguration();

builder.Services.AddCallbackFeatures();
builder.Services.AddHostedService<Consumer>();
builder.Services.AddLocalizerFactory();

var host = builder.Build();
host.Run();