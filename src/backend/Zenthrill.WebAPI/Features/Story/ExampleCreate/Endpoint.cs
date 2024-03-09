using Zenthrill.APIResponses;
using Zenthrill.Application.Features.Stories.ExampleCreate;

namespace Zenthrill.WebAPI.Features.Story.ExampleCreate;

public static class Endpoint
{
    public static async Task<IResult> CreateExample(
        Request request,
        IExampleStoryCreator storyCreator,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var createStoryRequest = mapper.MapToApplicationRequest(request);

        var storyInfoId = await storyCreator.CreateAsync(createStoryRequest, cancellationToken);

        return TypedResults.Ok(ApiResponses.Success(new Response { Id = storyInfoId.Value }));
    }
}