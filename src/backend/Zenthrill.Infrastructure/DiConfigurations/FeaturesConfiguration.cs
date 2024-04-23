using Zenthrill.Application.Features.Branches.Create;
using Zenthrill.Application.Features.Branches.Update;
using Zenthrill.Application.Features.Files.GetUploadLink;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.MarkEntrypoint;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.CreateVersion;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;
using Zenthrill.Application.Features.Stories.List;
using Zenthrill.Application.Features.Stories.Read;
using Zenthrill.Application.Features.Stories.ReadVersion;
using Zenthrill.Application.Features.Tags.List;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FeaturesConfiguration
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services
            .AddScoped<IStoryVersionCreator, StoryVersionCreator>()
            .AddScoped<IStoryReader, StoryReader>()
            .AddScoped<IStoryListReader, StoryListReader>()
            .AddScoped<IFragmentEntrypointMarker, FragmentEntrypointMarker>()
            .AddScoped<ITagListReader, TagListReader>();

        return services;
    }

    public static IServiceCollection AddCreateStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryCreator, StoryCreator>();
    }

    public static IServiceCollection AddCreateExampleStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IExampleStoryVersionCreator, ExampleStoryVersionCreator>();
    }

    public static IServiceCollection AddCallbackFeatures(this IServiceCollection services)
    {
        return services
            .AddScoped<IExampleStoryCreatorCallback, ExampleStoryVersionCreatorCallback>()
            .AddScoped<IStoryVersionCreatorCallback, StoryVersionCreatorCallback>();
    }

    public static IServiceCollection AddReadStoryFeature(this IServiceCollection services)
    {
        return services.AddScoped<IStoryVersionReader, StoryVersionVersionReader>();
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