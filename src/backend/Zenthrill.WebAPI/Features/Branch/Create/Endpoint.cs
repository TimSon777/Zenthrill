using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Branches;
using Zenthrill.Application.Features.Branches.Create;

namespace Zenthrill.WebAPI.Features.Branch.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        Request request,
        IBranchCreator branchCreator,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var createBranchRequest = mapper.MapToApplicationRequest(request);

        var result = await branchCreator.CreateAsync(createBranchRequest, cancellationToken);

        return result.Match<IResult>(
            branchId => TypedResults.Ok(ApiResponses.Success(new Response { Id = branchId.Value })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id.Value)),
            fragmentDoesNotExist => TypedResults.UnprocessableEntity(ApiResponses.Failure("fragment_does_not_exist")));
    }
}