using Zenthrill.Application.Features.Story;

namespace Zenthrill.WebAPI.Features.Story;

public interface IMapper
{
    CreateStoryRequest MapToApplicationRequest(Create.Request request);
}

public sealed class Mapper : IMapper
{
    public CreateStoryRequest MapToApplicationRequest(Create.Request request)
    {
        return new CreateStoryRequest
        {
            Name = request.Name,
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
}