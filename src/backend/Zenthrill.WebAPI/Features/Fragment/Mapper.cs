using System.Security.Claims;
using Zenthrill.Application.Features.Fragments.Create;
using Zenthrill.Application.Features.Fragments.MarkEntrypoint;
using Zenthrill.Application.Features.Fragments.Read;
using Zenthrill.Application.Features.Fragments.Update;
using Zenthrill.WebAPI.Common;
using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Fragment;

public interface IMapper
{
    ReadFragmentRequest MapToApplicationRequest(Read.Request request);
    
    CreateFragmentRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal);

    MarkFragmentEntrypointRequest MapToApplicationRequest(MarkEntrypoint.Request request, ClaimsPrincipal principal);

    UpdateFragmentRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal);

    Read.Response MapFromApplicationResponse(Domain.Entities.Fragment fragment);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public ReadFragmentRequest MapToApplicationRequest(Read.Request request)
    {
        return new ReadFragmentRequest
        {
            Id = new FragmentId(request.Id),
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId)
        };
    }

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

    public MarkFragmentEntrypointRequest MapToApplicationRequest(MarkEntrypoint.Request request, ClaimsPrincipal principal)
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

    public Read.Response MapFromApplicationResponse(Domain.Entities.Fragment fragment)
    {
        return new Read.Response
        {
            Fragment = new FragmentDto
            {
                Id = fragment.Id.Value,
                IsEntrypoint = fragment.IsEntrypoint,
                Body = fragment.Body,
                Name = fragment.Name
            },
            OutputBranches = fragment.OutputBranches.Select(b => new BranchDto
            {
                FromFragmentId = b.FromFragment.Id.Value,
                ToFragmentId = b.ToFragment.Id.Value,
                Id = b.Id.Value,
                Inscription = b.Inscription
            })
        };
    }
}