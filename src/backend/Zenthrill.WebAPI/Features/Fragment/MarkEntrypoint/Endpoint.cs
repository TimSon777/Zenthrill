using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragments.MarkEntrypoint;

namespace Zenthrill.WebAPI.Features.Fragment.MarkEntrypoint;

public static class Endpoint
{
    public static async Task<IResult> Mark(
        [FromBody] Request request,
        IFragmentEntrypointMarker fragmentEntrypointMarker,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var applicationRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await fragmentEntrypointMarker.MarkAsync(applicationRequest, cancellationToken);

        return result.Match<IResult>(
            success => TypedResults.Ok(ApiResponses.Success()),
            forbid => TypedResults.Forbid(),
            notFoundStoryVersionInfo => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundStoryVersionInfo.Id)),
            notFoundFragment => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundFragment.Id)),
            forbidEditBaseVersion => TypedResults.UnprocessableEntity(ApiResponses.Failure(StatusCodes.ForbidEditBaseVersion)));
    }
}