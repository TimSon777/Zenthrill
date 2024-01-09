using Zenthrill.Application.Features.Entrypoint;
using Zenthrill.Application.Features.Story;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FeaturesConfiguration
{
    public static IServiceCollection AddCreateStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryCreator, StoryCreator>();
    }

    public static IServiceCollection AddCreateEntrypointFeature(this IServiceCollection services)
    {
        return services.AddScoped<IEntrypointCreator, EntrypointCreator>();
    }
}