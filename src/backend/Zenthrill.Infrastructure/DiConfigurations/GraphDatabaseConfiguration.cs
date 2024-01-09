using Microsoft.Extensions.Hosting;
using Neo4j.Driver;
using Zenthrill.Application.Services;
using Zenthrill.Application.Settings;
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

        builder.Services.AddSingleton<IGraphDatabase, Zenthrill.Infrastructure.Services.GraphDatabase>();
        return builder;
    }
}