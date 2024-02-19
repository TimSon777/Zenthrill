using Zenthrill.Application.Features.Branch;
using Zenthrill.Application.Features.Fragment;
using Zenthrill.Application.Features.Story;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FeaturesConfiguration
{
    public static IServiceCollection AddCreateStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryCreator, StoryCreator>();
    }

    public static IServiceCollection AddReadStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryReader, StoryReader>();
    }

    public static IServiceCollection AddCreateBranchFeature(this IServiceCollection services)
    {
        return services.AddScoped<IBranchCreator, BranchCreator>();
    }

    public static IServiceCollection AddCreateFragmentFeature(this IServiceCollection services)
    {
        return services.AddScoped<IFragmentCreator, FragmentCreator>();
    }
}