using Zenthrill.Application.Features.Branches.Create;
using Zenthrill.Application.Features.Branches.Update;
using Zenthrill.Application.Features.Files.GetUploadLink;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.Application.Features.Stories;
using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.ExampleCreate;
using Zenthrill.Application.Features.Stories.Read;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FeaturesConfiguration
{
    public static IServiceCollection AddCreateStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryCreator, StoryCreator>();
    }

    public static IServiceCollection AddCreateExampleStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IExampleStoryCreator, ExampleStoryCreator>();
    }

    public static IServiceCollection AddCreateExampleStoryCallbackFeature(this IServiceCollection services)
    {
        return services.AddScoped<IExampleStoryCreatorCallback, ExampleStoryCreatorCallback>();
    }

    public static IServiceCollection AddReadStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryReader, StoryReader>();
    }

    public static IServiceCollection AddCreateBranchFeature(this IServiceCollection services)
    {
        return services.AddScoped<IBranchCreator, BranchCreator>();
    }

    public static IServiceCollection AddUpdateBranchFeature(this IServiceCollection services)
    {
        return services.AddScoped<IBranchUpdater, BranchUpdater>();
    }

    public static IServiceCollection AddGetUploadFileLinkFeature(this IServiceCollection services)
    {
        return services.AddScoped<IFileLinkUploadConstructor, FileLinkUploadConstructor>();
    }
    
    public static IServiceCollection AddFragmentFeatures(this IServiceCollection services)
    {
        return services
            .AddScoped<IFragmentUpdater, FragmentUpdater>()
            .AddScoped<IFragmentCreator, FragmentCreator>();
    }
}