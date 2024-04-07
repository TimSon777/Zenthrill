using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragments.Create;

namespace Zenthrill.WebAPI.Features.Fragment.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        [FromBody] Request request,
        IFragmentCreator fragmentCreator,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await fragmentCreator.CreateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            fragmentId => TypedResults.Ok(ApiResponses.Success(fragmentId.Value)),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id)),
            forbidEditBaseVersion => TypedResults.UnprocessableEntity(ApiResponses.Failure(StatusCodes.ForbidEditBaseVersion)));
    }
}