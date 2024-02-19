using Zenthrill.Application.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class LabelsConstructorConfiguration
{
    public static IServiceCollection AddLabelsConstructor(this IServiceCollection services)
    {
        return services.AddSingleton<ILabelsConstructor, LabelsConstructor>();
    }
}