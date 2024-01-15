using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Story;

namespace Zenthrill.WebAPI.Features.Story.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        Request request,
        IStoryCreator storyCreator,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request);

        var result = await storyCreator.CreateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            storyInfoId => TypedResults.Ok(ApiResponses.Success(new Response { Id = storyInfoId.Value })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)));
    }
}