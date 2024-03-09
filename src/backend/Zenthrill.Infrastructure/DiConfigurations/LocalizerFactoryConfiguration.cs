using TypesafeLocalization;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class LocalizerFactoryConfiguration
{
    public static IServiceCollection AddLocalizerFactory(this IServiceCollection services)
    {
        return services.AddSingleton<ILocalizerFactory, LocalizerFactory>();
    }
}