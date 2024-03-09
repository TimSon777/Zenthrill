using Microsoft.Extensions.Configuration;
using Zenthrill.Settings;

namespace Zenthrill.Application.Settings;

public sealed class GraphDatabaseSettings : ISettings
{
    public static string SectionName => "GRAPH_DATABASE";

    [ConfigurationKeyName("HOST")]
    public required Uri Host { get; set; }

    [ConfigurationKeyName("URI")]
    public required Uri Uri { get; set; }

    [ConfigurationKeyName("USERNAME")]
    public required string Username { get; set; }

    [ConfigurationKeyName("PASSWORD")]
    public required string Password { get; set; }
    
    [ConfigurationKeyName("DATABASE_NAME")]
    public required string DatabaseName { get; set; }
}