using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.Read;

namespace Zenthrill.WebAPI.Features.Story.ReadVersion;

public static class Endpoint
{
    public static async Task<IResult> ReadVersion(
        [AsParameters] Request request,
        IStoryReader storyReader,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var readStoryRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await storyReader.ReadAsync(readStoryRequest, cancellationToken);

        return result.Match<IResult>(
            storyResponse => TypedResults.Ok(ApiResponses.Success(mapper.MapFromApplicationResponse(storyResponse))),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id.Value)),
            forbid => TypedResults.Forbid());
    }
}