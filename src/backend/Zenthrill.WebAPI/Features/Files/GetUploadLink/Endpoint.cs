using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Files.GetUploadLink;

namespace Zenthrill.WebAPI.Features.Files.GetUploadLink;

public static class Endpoint
{
    public static async Task<IResult> GetUploadLink(
        Request request,
        IFileLinkUploadConstructor fileLinkUploadConstructor,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var getUploadLinkRequest = mapper.MapToApplicationRequest(request);

        var result = await fileLinkUploadConstructor.GetUploadLinkAsync(getUploadLinkRequest, cancellationToken);
        
        return result.Match<IResult>(
            uri => TypedResults.Ok(ApiResponses.Success(new Response { Uri = uri })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id)));
    }
}