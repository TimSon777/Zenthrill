using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragments.Update;

namespace Zenthrill.WebAPI.Features.Fragment.Update;

public static class Endpoint
{
    public static async Task<IResult> Update(
        [FromBody] Request request,
        IFragmentUpdater fragmentUpdater,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await fragmentUpdater.UpdateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            success => TypedResults.Ok(ApiResponses.Success()),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFoundStoryInfo => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundStoryInfo.Id)),
            notFoundFragment => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundFragment.Id)),
            forbidEditBaseVersion => TypedResults.UnprocessableEntity(ApiResponses.Failure(StatusCodes.ForbidEditBaseVersion)));
    }
}