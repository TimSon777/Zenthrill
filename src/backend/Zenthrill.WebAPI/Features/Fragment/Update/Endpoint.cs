using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragments.Update;

namespace Zenthrill.WebAPI.Features.Fragment.Update;

public static class Endpoint
{
    public static async Task<IResult> Update(
        Request request,
        IFragmentUpdater fragmentUpdater,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request);

        var result = await fragmentUpdater.UpdateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            success => TypedResults.Ok(ApiResponses.Success()),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFoundStoryInfo => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundStoryInfo.Id)),
            notFoundFragment => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFoundFragment.Id)));
    }
}