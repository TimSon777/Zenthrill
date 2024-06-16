using System.Security.Claims;
using Zenthrill.Application.Features.Files.GetUploadLink;
using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Files;

public interface IMapper
{
    GetUploadLinkRequest MapToApplicationRequest(GetUploadLink.Request request, ClaimsPrincipal principal);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public GetUploadLinkRequest MapToApplicationRequest(GetUploadLink.Request request, ClaimsPrincipal principal)
    {
        return new GetUploadLinkRequest
        {
            FileName = request.FileName,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }
}