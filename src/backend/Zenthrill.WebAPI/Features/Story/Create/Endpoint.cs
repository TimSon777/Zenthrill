using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.Create;

namespace Zenthrill.WebAPI.Features.Story.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        [FromBody] Request request,
        IStoryCreator storyCreator,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await storyCreator.CreateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            storyInfoId => TypedResults.Ok(ApiResponses.Success(new Response { Id = storyInfoId.Value })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)));
    }
}