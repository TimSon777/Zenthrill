using Zenthrill.Settings;

namespace Zenthrill.WebAPI.Settings;

public sealed class IdentityServerSettings : ISettings
{
    public static string SectionName => "IDENTITY_SERVER";

    [ConfigurationKeyName("URI")]
    public Uri Uri { get; set; } = default!;

    [ConfigurationKeyName("SIGNING_KEY")]
    public string SingingKey { get; set; } = default!;
}