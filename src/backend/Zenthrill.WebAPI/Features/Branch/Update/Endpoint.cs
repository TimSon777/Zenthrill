using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Branches.Update;

namespace Zenthrill.WebAPI.Features.Branch.Update;

public static class Endpoint
{
    public static async Task<IResult> Update(
        [FromBody] Request request,
        IBranchUpdater branchUpdater,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createBranchRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await branchUpdater.UpdateAsync(createBranchRequest, cancellationToken);

        return result.Match<IResult>(
            fragmentId => TypedResults.Ok(ApiResponses.Success(new Response { Id = fragmentId.Value })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFoundStoryInfoId => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundStoryInfoId.Id.Value)),
            notFoundBranchId => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundBranchId.Id.Value)),
            forbidEditBaseVersion => TypedResults.UnprocessableEntity(ApiResponses.Failure(StatusCodes.ForbidEditBaseVersion)));
    }
}