using Zenthrill.Application.Features.Files.GetUploadLink;

namespace Zenthrill.WebAPI.Features.Files;

public interface IMapper
{
    GetUploadLinkRequest MapToApplicationRequest(GetUploadLink.Request request);
}

public sealed class Mapper : IMapper
{
    public GetUploadLinkRequest MapToApplicationRequest(GetUploadLink.Request request)
    {
        return new GetUploadLinkRequest
        {
            Extension = request.Extension,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
}