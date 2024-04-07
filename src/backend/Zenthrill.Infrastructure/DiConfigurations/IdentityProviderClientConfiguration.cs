using Microsoft.Extensions.Hosting;
using Zenthrill.Application.Clients;
using Zenthrill.Application.Settings;
using Zenthrill.Settings.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityProviderClientConfiguration
{
    public static IHostApplicationBuilder AddIdentityProviderClientConfiguration(this IHostApplicationBuilder builder)
    {
        builder.ConfigureSettings<IdentityProviderSettings>();

        builder.Services.AddSingleton<IIdentityProviderClient, IdentityProviderClient>();

        return builder;
    }
}