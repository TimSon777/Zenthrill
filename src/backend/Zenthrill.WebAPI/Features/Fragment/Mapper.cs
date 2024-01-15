using Zenthrill.Application.Features.Fragment;

namespace Zenthrill.WebAPI.Features.Fragment;

public interface IMapper
{
    CreateFragmentRequest MapToApplicationRequest(Create.Request request);
}

public sealed class Mapper : IMapper
{
    public CreateFragmentRequest MapToApplicationRequest(Create.Request request)
    {
        return new CreateFragmentRequest
        {
            Body = request.Body,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
}