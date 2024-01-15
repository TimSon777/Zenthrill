using Microsoft.Extensions.Configuration;
using Zenthrill.Settings;

namespace Zenthrill.Application.Settings;

public sealed class MainDatabaseSettings : ISettings
{
    public static string SectionName => "MAIN_DATABASE";

    [ConfigurationKeyName("CONNECTION_STRING")]
    public required string ConnectionString { get; set; }

    [ConfigurationKeyName("ENABLE_SENSITIVE_DATA_LOGGING")]
    public required bool EnableSensitiveDataLogging { get; set; }
}