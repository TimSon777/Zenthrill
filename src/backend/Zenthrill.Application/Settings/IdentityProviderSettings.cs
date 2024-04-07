using Microsoft.Extensions.Configuration;
using Zenthrill.Domain.Entities;
using Zenthrill.Settings;

namespace Zenthrill.Application.Settings;

public sealed class IdentityProviderSettings : ISettings
{
    public static string SectionName => "IDENTITY_PROVIDER";
    
    [ConfigurationKeyName("URI")]
    public required Uri Uri { get; set; }

    public Uri GetUserUri(UserId userId)
    {
        return new Uri(Uri, $"users/{userId.Value}");
    }
}