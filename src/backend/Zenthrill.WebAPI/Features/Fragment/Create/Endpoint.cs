using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Fragment;

namespace Zenthrill.WebAPI.Features.Fragment.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        Request request,
        IFragmentCreator fragmentCreator,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request);

        var result = await fragmentCreator.CreateAsync(createStoryRequest, cancellationToken);

        return result.Match<IResult>(
            fragmentId => TypedResults.Ok(ApiResponses.Success(fragmentId.Value)),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id)));
    }
}