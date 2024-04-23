using Microsoft.AspNetCore.Mvc;
using Zenthrill.UserStory.Model.Dto;
using Zenthrill.UserStory.Model.Services;
using ExecuteStepRequest = Zenthrill.UserStory.WebAPI.Requests.ExecuteStepRequest;

namespace Zenthrill.UserStory.WebAPI.Controllers;

[Route("story-runtime")]
public sealed class StoryRuntimeController(
    IUserProcessor userProcessor,
    IStoryRuntime storyRuntime) : ControllerBase
{
    [HttpPost]
    public async Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request, CancellationToken cancellationToken)
    {
        var applicationRequest = new Model.Dto.ExecuteStepRequest
        {
            StoryInfoVersionId = request.StoryInfoVersionId,
            BranchId = request.BranchId,
            User = await userProcessor.ProcessUserAsync(HttpContext.User, HttpContext.Request.Headers.UserAgent!, cancellationToken)
        };

        return await storyRuntime.ExecuteStepAsync(applicationRequest, cancellationToken);
    }
}