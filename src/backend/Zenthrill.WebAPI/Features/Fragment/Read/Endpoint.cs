using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragments.Read;

namespace Zenthrill.WebAPI.Features.Fragment.Read;

public static class Endpoint
{
    public static async Task<IResult> Read(
        [AsParameters] Request request,
        IFragmentReader fragmentReader,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var applicationRequest = mapper.MapToApplicationRequest(request);

        var result = await fragmentReader.ReadAsync(applicationRequest, cancellationToken);

        return result.Match<IResult>(
            fragment => TypedResults.Ok(ApiResponses.Success(mapper.MapFromApplicationResponse(fragment))),
            notFoundStoryInfoVersion => TypedResults.NotFound(ApiResponses.NotFound(notFoundStoryInfoVersion.Id)),
            notFoundFragment => TypedResults.NotFound(ApiResponses.NotFound(notFoundFragment.Id)));
    }
}