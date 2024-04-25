using Microsoft.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Clients;
using Zenthrill.UserStory.Model.Dto;
using Zenthrill.UserStory.Model.Entities;
using Zenthrill.UserStory.Model.Infrastructure.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Models;

namespace Zenthrill.UserStory.Model.Services;

public interface IStoryRuntime
{
    Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request, CancellationToken cancellationToken);
}

public sealed class StoryRuntime(
    IStoryClient storyClient,
    ApplicationDbContext applicationDbContext) : IStoryRuntime
{
    public async Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request, CancellationToken cancellationToken)
    {
        var response = await storyClient.GetStoryVersionAsync(request.StoryInfoVersionId, cancellationToken);

        var existedStory = await applicationDbContext.Stories
            .FirstOrDefaultAsync(
                s => s.StoryInfoVersionId == request.StoryInfoVersionId
                     && s.User.Id == request.User.Id,
                cancellationToken);

        var branches = response!.Value.Components
            .SelectMany(c => c.Branches)
            .ToArray();

        var branch = request.BranchId is null
            ? null
            : response.Value.Components.SelectMany(c => c.Branches).First(b => b.Id == request.BranchId);

        var fragment = branch is null
            ? response.Value.Components.SelectMany(c => c.Fragments).First(f => f.Id == response.Value.EntrypointFragmentId)
            : response.Value.Components.SelectMany(c => c.Fragments).First(f => f.Id == branch.ToFragmentId);

        var outboundBranches = branches.Where(b => b.FromFragmentId == fragment.Id);
        
        if (existedStory is null)
        {
            var story = new Story
            {
                StoryInfoVersionId = request.StoryInfoVersionId,
                User = request.User,
                ExecutionContext = new StoryExecutionContext
                {
                    CurrentFragmentId = fragment.Id
                }
            };

            await applicationDbContext.Stories.AddAsync(story, cancellationToken);
        }
        else
        {
            applicationDbContext.Stories.Update(existedStory);
            existedStory.ExecutionContext.CurrentFragmentId = fragment.Id;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new ExecuteStepResponse
        {
            Fragment = new Fragment
            {
                Body = fragment.Body,
                Name = fragment.Name,
                Id = fragment.Id
            },
            OutputBranches = outboundBranches.Select(b => new Branch
            {
                Inscription = b.Inscription,
                Id = b.Id,
                FromFragmentId = b.FromFragmentId,
                ToFragmentId = b.ToFragmentId
            })
        };
    }
}

