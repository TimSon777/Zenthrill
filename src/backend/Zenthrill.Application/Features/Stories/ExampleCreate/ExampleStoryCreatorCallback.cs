using Microsoft.EntityFrameworkCore;
using TypesafeLocalization;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleCreate;

public interface IExampleStoryCreatorCallback
{
    Task CreateCallbackAsync(ExampleStoryCreateCallbackRequest request, CancellationToken cancellationToken);
}

public sealed class ExampleStoryCreatorCallback(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext,
    ILocalizerFactory localizerFactory) : IExampleStoryCreatorCallback
{
    public async Task CreateCallbackAsync(ExampleStoryCreateCallbackRequest request, CancellationToken cancellationToken)
    {
        var storyInfo = await applicationDbContext.StoryInfos
            .FirstAsync(si => si.Id == request.StoryInfoId, cancellationToken);

        var story = CreateStory(storyInfo, request.Locale);

        var branches = story.TraverseBranches();

        await graphDbContext.ExecuteAsync(async (repositoryRegistry, ct) =>
        {
            foreach (var branch in branches)
            {
                await repositoryRegistry.BranchRepository
                    .CreateAsync(branch, storyInfo.Id);
            }
        }, cancellationToken);
    }
    
    private Story CreateStory(StoryInfo storyInfo, Locale locale)
    {
        var localizer = localizerFactory.CreateLocalizer(locale);

        var entrypoint = new Fragment(storyInfo.EntrypointFragmentId!.Value)
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
        
        var story = new Story
        {
            StoryInfo = storyInfo
        };

        story.AddComponent(entrypoint);

        return story;
    }
}