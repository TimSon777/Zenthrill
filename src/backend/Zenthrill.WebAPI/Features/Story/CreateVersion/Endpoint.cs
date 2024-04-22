using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.CreateVersion;

namespace Zenthrill.WebAPI.Features.Story.CreateVersion;

public static class Endpoint
{
    public static async Task<IResult> CreateVersion(
        [FromBody] Request request,
        IStoryVersionCreator storyVersionCreator,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var user = httpContext.User;
        var createStoryVersionRequest = mapper.MapToApplicationRequest(request, user);

        var result = await storyVersionCreator.CreateAsync(createStoryVersionRequest, cancellationToken);

        return result.Match<IResult>(
            storyVersionId => TypedResults.Ok(ApiResponses.Success(new Response { Id = storyVersionId.Value })),
            notFoundStory => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundStory.Id)),
            notFoundVersion => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundVersion.Id)),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid());
    }
}