using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zenthrill.APIResponses;
using Zenthrill.UserStory.Model.Clients;
using Zenthrill.UserStory.Model.Clients.Dto;
using Zenthrill.UserStory.Model.Dto;
using Zenthrill.UserStory.Model.Infrastructure.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Services;

namespace Zenthrill.UserStory.WebAPI.Controllers;

[Route("stories")]
public sealed class StoryController(
    IStoryClient storyClient,
    IUserProcessor userProcessor,
    ApplicationDbContext applicationDbContext) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse> GetExecutionContextAsync(Guid id, CancellationToken cancellationToken)
    {
        var storyVersionResponse = await storyClient.GetStoryVersionAsync(id, cancellationToken);

        var user = await userProcessor.ProcessUserAsync(
            HttpContext.User,
            HttpContext.Request.Headers.UserAgent!,
            cancellationToken);
        
        var story = await applicationDbContext.Stories.FirstOrDefaultAsync(
            s => s.StoryInfoVersionId == id && s.User == user,
            cancellationToken);

        FragmentResponse? fragmentResponse = null;
        if (story is not null)
        {
            fragmentResponse = await storyClient.GetFragmentAsync(
                id,
                story.ExecutionContext.CurrentFragmentId,
                cancellationToken);
        }
        
        return ApiResponses.Success(new Response
        {
            Name = storyVersionResponse.Value.Name,
            Fragment = fragmentResponse?.Value.Fragment,
            OutputBranches = fragmentResponse?.Value.OutputBranches
        });
    }
}

file sealed class Response
{
    public required string Name { get; set; }

    public required FragmentDto? Fragment { get; set; }
    
    public required IEnumerable<BranchDto>? OutputBranches { get; set; }
}
