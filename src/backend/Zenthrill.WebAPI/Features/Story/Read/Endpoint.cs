﻿using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Story;

namespace Zenthrill.WebAPI.Features.Story.Read;

public static class Endpoint
{
    public static async Task<IResult> Read(
        [AsParameters] Request request,
        IStoryReader storyReader,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var readStoryRequest = mapper.MapToApplicationRequest(request);

        var result = await storyReader.ReadAsync(readStoryRequest, cancellationToken);

        return result.Match<IResult>(
            storyResponse => TypedResults.Ok(ApiResponses.Success(mapper.MapFromApplicationResponse(storyResponse))),
            notFound => TypedResults.UnprocessableEntity(ApiResponses.NotFound(notFound.Id.Value)),
            forbid => TypedResults.Forbid());
    }
}