using System.Security.Claims;
using Zenthrill.Application.Features.Fragments;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Fragment;

public interface IMapper
{
    CreateFragmentRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal);

    UpdateFragmentRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public CreateFragmentRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal)
    {
        return new CreateFragmentRequest
        {
            Body = request.Body,
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }

    public UpdateFragmentRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal)
    {
        return new UpdateFragmentRequest
        {
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            Body = request.Body,
            FragmentId = new FragmentId(request.FragmentInfoId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }
}