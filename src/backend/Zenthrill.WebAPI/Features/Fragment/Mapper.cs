using System.Security.Claims;
using Zenthrill.Application.Features.Fragments;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.MarkEntrypoint;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.WebAPI.Common;
using Zenthrill.WebAPI.Features.Fragment.MarkEntrypoint;

namespace Zenthrill.WebAPI.Features.Fragment;

public interface IMapper
{
    CreateFragmentRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal);

    MarkFragmentEntrypointRequest MapToApplicationRequest(MarkEntrypoint.Request request, ClaimsPrincipal principal);

    UpdateFragmentRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public CreateFragmentRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal)
    {
        return new CreateFragmentRequest
        {
            Name = request.Name,
            Body = request.Body,
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            User = userMapper.MapToApplicationUser(principal),
            FromFragmentId = request.FromFragmentId is not null
                ? new FragmentId(request.FromFragmentId.Value)
                : null
        };
    }

    public MarkFragmentEntrypointRequest MapToApplicationRequest(Request request, ClaimsPrincipal principal)
    {
        return new MarkFragmentEntrypointRequest
        {
            Id = new FragmentId(request.Id),
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }

    public UpdateFragmentRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal)
    {
        return new UpdateFragmentRequest
        {
            Name = request.Name,
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            Body = request.Body,
            FragmentId = new FragmentId(request.FragmentId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }
}