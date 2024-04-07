using System.Security.Claims;
using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.CreateVersion;
using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;
using Zenthrill.Application.Features.Stories.ReadVersion;
using Zenthrill.WebAPI.Common;
using Zenthrill.WebAPI.Features.Story.ExampleVersionCreate;
using Zenthrill.WebAPI.Features.Story.ReadVersion.Objects;
using Response = Zenthrill.WebAPI.Features.Story.ReadVersion.Response;

namespace Zenthrill.WebAPI.Features.Story;

public interface IMapper
{
    ExampleStoryVersionCreateRequest MapToApplicationRequest(Request request, ClaimsPrincipal user);

    CreateStoryRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal user);
    
    ReadStoryVersionRequest MapToApplicationRequest(ReadVersion.Request request, ClaimsPrincipal user);

    CreateStoryVersionRequest MapToApplicationRequest(CreateVersion.Request request, ClaimsPrincipal user);

    Response MapFromApplicationResponse(Domain.Aggregates.Story story);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public ExampleStoryVersionCreateRequest MapToApplicationRequest(Request request, ClaimsPrincipal user)
    {
        return new ExampleStoryVersionCreateRequest
        {
            Locale = request.Locale,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = userMapper.MapToApplicationUser(user)
        };
    }

    public CreateStoryRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal user)
    {
        return new CreateStoryRequest
        {
            Description = request.Name,
            User = userMapper.MapToApplicationUser(user)
        };
    }

    public ReadStoryVersionRequest MapToApplicationRequest(ReadVersion.Request request, ClaimsPrincipal user)
    {
        return new ReadStoryVersionRequest
        {
            StoryInfoVersionId = new StoryInfoVersionId(request.Id),
            User = userMapper.MapToApplicationUser(user)
        };
    }
    
    public Response MapFromApplicationResponse(Domain.Aggregates.Story story)
    {
        return new Response
        {
            StoryInfo = new StoryInfoDto
            {
                Id = story.StoryInfoVersion.Id.Value,
                Description = story.StoryInfoVersion.StoryInfo.Description
            },
            Name = story.StoryInfoVersion.Name,
            Components = story.Components.Select(c => new ComponentDto
            {
                Branches = c.TraverseBranches().Select(MapToBranchDto),
                Fragments = c.TraverseFragments().Select(MapToFragmentDto)
            })
        };
    }

    public CreateStoryVersionRequest MapToApplicationRequest(CreateVersion.Request request,
        ClaimsPrincipal user)
    {
        return new CreateStoryVersionRequest
        {
            BaseStoryInfoVersionId = new StoryInfoVersionId(request.BaseStoryInfoVersionId),
            Name = request.Name,
            User = userMapper.MapToApplicationUser(user),
            Version = new StoryVersionDto
            {
                Major = request.Version.Major,
                Minor = request.Version.Minor,
                Suffix = request.Version.Suffix
            }
        };
    }
  
    private static FragmentDto MapToFragmentDto(Domain.Entities.Fragment fragment)
    {
        return new FragmentDto
        {
            Id = fragment.Id.Value,
            IsEntrypoint = fragment.IsEntrypoint,
            Body = fragment.Body
        };
    }
    
    private static BranchDto MapToBranchDto(Domain.Entities.Branch branch)
    {
        return new BranchDto
        {
            Id = branch.Id.Value,
            Inscription = branch.Inscription,
            FromFragmentId = branch.FromFragment.Id.Value,
            ToFragmentId = branch.ToFragment.Id.Value
        };
    }
}