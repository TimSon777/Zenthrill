using System.Security.Claims;
using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.CreateVersion;
using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.Application.Features.Stories.ExampleVersionCreate;
using Zenthrill.Application.Features.Stories.List;
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

    ListStoryRequest MapToApplicationRequest(ClaimsPrincipal user);

    List.Response MapFromApplicationResponse(IEnumerable<StoryInfo> storyInfos);

    ReadVersion.Response MapFromApplicationResponse(Domain.Aggregates.StoryVersion storyVersion);

    Read.Response MapFromApplicationResponse(Domain.Aggregates.Story story);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public ExampleStoryVersionCreateRequest MapToApplicationRequest(Request request, ClaimsPrincipal user)
    {
        return new ExampleStoryVersionCreateRequest
        {
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

    public ListStoryRequest MapToApplicationRequest(ClaimsPrincipal user)
    {
        return new ListStoryRequest
        {
            User = userMapper.MapToApplicationUser(user)
        };
    }

    public List.Response MapFromApplicationResponse(IEnumerable<StoryInfo> storyInfos)
    {
        return new List.Response
        {
            StoryInfos = storyInfos.Select(si => new StoryInfoDto
            {
                Id = si.Id.Value,
                Description = si.Description
            })
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
            }),
            Version = MapToVersionDto(storyVersion.StoryInfoVersion.Version),
            Id = storyVersion.StoryInfoVersion.Id.Value
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
                Name = v.Name,
                Version = MapToVersionDto(v.Version)
            })
        };
    }

    public CreateStoryVersionRequest MapToApplicationRequest(CreateVersion.Request request,
        ClaimsPrincipal user)
    {
        return new CreateStoryVersionRequest
        {
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            BaseStoryInfoVersionId = request.BaseStoryInfoVersionId.HasValue
                ? new StoryInfoVersionId(request.BaseStoryInfoVersionId.Value)
                : null,
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
            Body = fragment.Body,
            Name = fragment.Name
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

    private static VersionDto MapToVersionDto(StoryVersion version)
    {
        return new VersionDto
        {
            Major = version.Major,
            Minor = version.Minor,
            Suffix = version.Suffix
        };
    }
}