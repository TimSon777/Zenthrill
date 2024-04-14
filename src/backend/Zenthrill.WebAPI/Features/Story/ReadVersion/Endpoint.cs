using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.Read;
using Zenthrill.Application.Features.Stories.ReadVersion;

namespace Zenthrill.WebAPI.Features.Story.ReadVersion;

public static class Endpoint
{
    public static async Task<IResult> ReadVersion(
        [AsParameters] Request request,
        IStoryVersionReader storyVersionReader,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var readStoryRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await storyVersionReader.ReadAsync(readStoryRequest, cancellationToken);

        return result.Match<IResult>(
            storyResponse => TypedResults.Ok(ApiResponses.Success(mapper.MapFromApplicationResponse(storyResponse))),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id.Value)),
            forbid => TypedResults.Forbid());
    }
}