using Microsoft.Extensions.Hosting;
using Neo4j.Driver;
using Neo4jClient;
using Zenthrill.Application;
using Zenthrill.Application.Interfaces;
using Zenthrill.Application.Repositories;
using Zenthrill.Application.Settings;
using Zenthrill.Infrastructure.GraphDatabase.Repositories;
using Zenthrill.Infrastructure.Services;
using Zenthrill.Settings.DependencyInjection;
using GraphDatabase = Neo4j.Driver.GraphDatabase;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class GraphDatabaseConfiguration
{
    public static IHostApplicationBuilder AddGraphDatabaseConfiguration(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<GraphDatabaseSettings>();

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetOptions<GraphDatabaseSettings>();
            var credentials = AuthTokens.Basic(settings.Username, settings.Password);
            return GraphDatabase.Driver(settings.Host, credentials);
        });

        builder.Services.AddSingleton<BoltGraphClient>(sp =>
        {
            var settings = sp.GetOptions<GraphDatabaseSettings>();
            var client = new BoltGraphClient(settings.Uri, settings.Username, settings.Password);
            client.ConnectAsync().Wait();
            return client;
        });

        builder.Services.AddSingleton<IGraphDbContext, GraphDbContext>();

        builder.Services.AddSingleton<IRepositoryRegistry, RepositoryRegistry>();

        builder.Services.AddSingleton<IFragmentRepository, FragmentRepository>();
        builder.Services.AddSingleton<IStoryRepository, StoryRepository>();
        builder.Services.AddSingleton<IBranchRepository, BranchRepository>();

        builder.Services.AddSingleton<ILabelsConverter, LabelsConverter>();

        return builder;
    }
}