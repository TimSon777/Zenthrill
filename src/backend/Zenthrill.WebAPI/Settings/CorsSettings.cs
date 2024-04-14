using Zenthrill.Settings;

namespace Zenthrill.WebAPI.Settings;

public sealed class CorsSettings : ISettings
{
    public static string SectionName => "CORS";

    [ConfigurationKeyName("ORIGINS")]
    public IEnumerable<string> Origins { get; set; } = default!;
}