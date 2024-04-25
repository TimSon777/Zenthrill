using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
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
    public async Task<ApiResponse> ExecuteStepAsync([FromBody] ExecuteStepRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"A{request.StoryInfoVersionId}");
        var applicationRequest = new Model.Dto.ExecuteStepRequest
        {
            StoryInfoVersionId = request.StoryInfoVersionId,
            BranchId = request.BranchId,
            User = await userProcessor.ProcessUserAsync(HttpContext.User, HttpContext.Request.Headers.UserAgent!, cancellationToken)
        };

        var response = await storyRuntime.ExecuteStepAsync(applicationRequest, cancellationToken);

        return ApiResponses.Success(response);
    }
}