using Microsoft.EntityFrameworkCore;
using TypesafeLocalization;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleVersionCreate;

public interface IExampleStoryCreatorCallback
{
    Task CreateCallbackAsync(ExampleStoryVersionCreateCallbackRequest request, CancellationToken cancellationToken);
}

public sealed class ExampleStoryVersionCreatorCallback(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    ILocalizerFactory localizerFactory) : IExampleStoryCreatorCallback
{
    public async Task CreateCallbackAsync(ExampleStoryVersionCreateCallbackRequest request, CancellationToken cancellationToken)
    {
        var storyInfoVersion = await applicationDbContext.StoryInfoVersions
            .FirstAsync(siv => siv.Id == request.StoryInfoVersionId, cancellationToken);

        var story = CreateStoryVersion(storyInfoVersion, request.Locale);

        var branches = story.TraverseBranches();

        await graphDbContext.ExecuteAsync(async (repositoryRegistry, _) =>
        {
            foreach (var branch in branches)
            {
                await repositoryRegistry.BranchRepository
                    .CreateAsync(branch, storyInfoVersion.Id);
            }
        }, cancellationToken);
    }
    
    private StoryVersion CreateStoryVersion(StoryInfoVersion storyInfoVersion, Locale locale)
    {
        var localizer = localizerFactory.CreateLocalizer(locale);

        var entrypoint = new Fragment(storyInfoVersion.EntrypointFragmentId!.Value)
        {
            IsEntrypoint = true,
            Body = localizer.ExampleStoryEntrypointBody()
        };

        var fragment1 = new Fragment
        {
            Body = localizer.ExampleStoryFragment1Body()
        };

        var fragment2 = new Fragment
        {
            Body = localizer.ExampleStoryFragment2Body()
        };

        var fragment3 = new Fragment
        {
            Body = localizer.ExampleStoryFragment3Body()
        };

        _ = new Branch(entrypoint, fragment1)
        {
            Inscription = localizer.ExampleStoryBranchEntrypointToFragment1()
        };

        _ = new Branch(entrypoint, fragment2)
        {
            Inscription = localizer.ExampleStoryBranchEntrypointToFragment2()
        };
        
        _ = new Branch(fragment1, fragment3)
        {
            Inscription = localizer.ExampleStoryBranchFragment1ToFragment3()
        };
        
        _ = new Branch(fragment2, fragment3)
        {
            Inscription = localizer.ExampleStoryBranchFragment2ToFragment3()
        };
        
        var story = new StoryVersion
        {
            StoryInfoVersion = storyInfoVersion
        };

        story.AddComponent(entrypoint);

        return story;
    }
}