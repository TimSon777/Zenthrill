using Zenthrill.Providers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DateTimeOffsetProviderConfiguration
{
    public static IServiceCollection AddDateTimeOffsetProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();
        return services;
    }
}