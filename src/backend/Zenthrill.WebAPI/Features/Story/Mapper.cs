using System.Security.Claims;
using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.CreateVersion;
using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;
using Zenthrill.Application.Features.Stories.Read;
using Zenthrill.Application.Features.Stories.ReadVersion;
using Zenthrill.WebAPI.Common;
using Zenthrill.WebAPI.Features.Story.ExampleVersionCreate;
using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Story;

public interface IMapper
{
    ExampleStoryVersionCreateRequest MapToApplicationRequest(Request request, ClaimsPrincipal user);

    CreateStoryRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal user);
    
    ReadStoryVersionRequest MapToApplicationRequest(ReadVersion.Request request, ClaimsPrincipal user);

    CreateStoryVersionRequest MapToApplicationRequest(CreateVersion.Request request, ClaimsPrincipal user);

    ReadStoryRequest MapToApplicationRequest(Read.Request request, ClaimsPrincipal user);

    ReadVersion.Response MapFromApplicationResponse(Domain.Aggregates.StoryVersion storyVersion);

    Read.Response MapFromApplicationResponse(Domain.Aggregates.Story story);
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
            Description = request.Description,
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

    public ReadStoryRequest MapToApplicationRequest(Read.Request request, ClaimsPrincipal user)
    {
        return new ReadStoryRequest
        {
            StoryInfoId = new StoryInfoId(request.Id),
            User = userMapper.MapToApplicationUser(user)
        };
    }

    public ReadVersion.Response MapFromApplicationResponse(Domain.Aggregates.StoryVersion storyVersion)
    {
        return new ReadVersion.Response
        {
            StoryInfo = new StoryInfoDto
            {
                Id = storyVersion.StoryInfoVersion.Id.Value,
                Description = storyVersion.StoryInfoVersion.StoryInfo.Description
            },
            Name = storyVersion.StoryInfoVersion.Name,
            Components = storyVersion.Components.Select(c => new ComponentDto
            {
                Branches = c.TraverseBranches().Select(MapToBranchDto),
                Fragments = c.TraverseFragments().Select(MapToFragmentDto)
            })
        };
    }

    public Read.Response MapFromApplicationResponse(Domain.Aggregates.Story story)
    {
        return new Read.Response
        {
            StoryInfo = new StoryInfoDto
            {
                Id = story.StoryInfoId.Value,
                Description = story.Description
            },
            Versions = story.Versions.Select(v => new StoryVersionDto
            {
                Id = v.Id.Value,
                Name = v.Name
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
            Version = new VersionDto
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