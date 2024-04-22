using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.List;

namespace Zenthrill.WebAPI.Features.Story.List;

public static class Endpoint
{
    public static async Task<IResult> List(
        IStoryListReader storyListReader,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var applicationRequest = mapper.MapToApplicationRequest(httpContext.User);
        var storyInfos = await storyListReader.ListAsync(applicationRequest, cancellationToken);

        return TypedResults.Ok(ApiResponses.Success(mapper.MapFromApplicationResponse(storyInfos)));
    }
}