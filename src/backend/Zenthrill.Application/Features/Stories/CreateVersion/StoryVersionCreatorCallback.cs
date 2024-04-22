using Microsoft.EntityFrameworkCore;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public interface IStoryVersionCreatorCallback
{
    Task CreateCallbackAsync(CreateStoryVersionCallbackRequest request, CancellationToken cancellationToken);
}

public sealed class StoryVersionCreatorCallback(
    IApplicationDbContext applicationDbContext,
    IGraphDbContext graphDbContext) : IStoryVersionCreatorCallback
{
    public async Task CreateCallbackAsync(CreateStoryVersionCallbackRequest request, CancellationToken cancellationToken)
    {
        var storyInfoVersion = await applicationDbContext.StoryInfoVersions
            .Include(siv => siv.StoryInfo)
            .Include(siv => siv.BaseVersion)
            .FirstAsync(siv => siv.Id == request.StoryInfoVersionId,
                cancellationToken);

        if (storyInfoVersion.BaseVersion is null)
        {
            throw new InvalidOperationException($"BaseVersion is required. StoryInfoVersionId = {storyInfoVersion.Id.Value}");
        }

        await graphDbContext.ExecuteAsync(
            async (repositoryRegistry, ct) =>
            {
                var baseStoryVersion = await repositoryRegistry.StoryRepository
                    .ReadAsync(storyInfoVersion.BaseVersion, ct);

                var oldFragments = baseStoryVersion
                    .TraverseFragments()
                    .ToHashSet();

                var newFragments = oldFragments
                    .ToDictionary(f => f.Id, f =>
                    {
                        if (f.IsEntrypoint)
                        {
                            return new Fragment(storyInfoVersion.EntrypointFragmentId!.Value)
                            {
                                Body = f.Body,
                                Name = f.Name,
                                IsEntrypoint = false
                            }; 
                        }

                        return new Fragment
                        {
                            Body = f.Body,
                            Name = f.Name,
                            IsEntrypoint = true
                        };
                    });

                var branches = baseStoryVersion
                    .TraverseBranches()
                    .Select(b =>
                        new Branch(newFragments[b.FromFragment.Id], newFragments[b.ToFragment.Id])
                        {
                            Inscription = b.Inscription
                        });

                Console.WriteLine("AAAA");
                foreach (var branch in branches)
                {
                    Console.WriteLine(branch.Id);
                    await repositoryRegistry.BranchRepository
                        .CreateAsync(branch, storyInfoVersion.Id);
                }
            },
            cancellationToken);
    }
}