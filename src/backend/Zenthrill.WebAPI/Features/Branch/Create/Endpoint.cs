using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Branches.Create;
using Zenthrill.Application.Results;

namespace Zenthrill.WebAPI.Features.Branch.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(
        [FromBody] Request request,
        IBranchCreator branchCreator,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createBranchRequest = mapper.MapToApplicationRequest(request, httpContext.User);

        var result = await branchCreator.CreateAsync(createBranchRequest, cancellationToken);

        return result.Match<IResult>(
            branchId => TypedResults.Ok(ApiResponses.Success(new Response { Id = branchId.Value })),
            validationFailure => TypedResults.BadRequest(ApiResponses.Failure(DefaultStatusCodes.BadRequest, validationFailure.Errors)),
            forbid => TypedResults.Forbid(),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id.Value)),
            fragmentDoesNotExist => TypedResults.UnprocessableEntity(ApiResponses.Failure("fragment_does_not_exist")),
            forbidEditBaseVersion => TypedResults.UnprocessableEntity(ApiResponses.Failure(StatusCodes.ForbidEditBaseVersion)));
    }
}