using Zenthrill.Application.Features.Story;
using Zenthrill.Domain.Entities;

namespace Zenthrill.WebAPI.Features.Story.Create;

public static class Endpoint
{
    public static async Task<IResult> Create(Request request, IStoryCreator storyCreator, CancellationToken cancellationToken)
    {
        var createStoryRequest = new CreateStoryRequest
        {
            Name = request.Name,
            User = new User
            {
                Nickname = "Test",
                Id = new UserId { Id = new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6") }
            }
        };

        var result = await storyCreator.CreateAsync(createStoryRequest, cancellationToken);

        return TypedResults.Ok(result.Value);
    }
}