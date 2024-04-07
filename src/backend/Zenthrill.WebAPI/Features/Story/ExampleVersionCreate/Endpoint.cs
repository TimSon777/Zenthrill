using Microsoft.AspNetCore.Mvc;
using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;

namespace Zenthrill.WebAPI.Features.Story.ExampleVersionCreate;

public static class Endpoint
{
    public static async Task<IResult> CreateExampleVersion(
        [FromBody] Request request,
        IExampleStoryVersionCreator storyVersionCreator,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var user = httpContext.User;
        var createStoryRequest = mapper.MapToApplicationRequest(request, user);

        var storyInfoId = await storyVersionCreator.CreateAsync(createStoryRequest, cancellationToken);

        return TypedResults.Ok(ApiResponses.Success(new Response { Id = storyInfoId.Value }));
    }
}