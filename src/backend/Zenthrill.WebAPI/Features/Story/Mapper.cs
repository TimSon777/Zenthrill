using Zenthrill.Application.Features.Stories.Create;
using Zenthrill.Application.Features.Stories.ExampleCreate;
using Zenthrill.Application.Features.Stories.Read;
using Zenthrill.WebAPI.Features.Story.Read.Objects;

namespace Zenthrill.WebAPI.Features.Story;

public interface IMapper
{
    ExampleStoryCreateRequest MapToApplicationRequest(ExampleCreate.Request request);

    CreateStoryRequest MapToApplicationRequest(Create.Request request);
    
    ReadStoryRequest MapToApplicationRequest(Read.Request request);

    Read.Response MapFromApplicationResponse(Domain.Aggregates.Story story);
}

public sealed class Mapper : IMapper
{
    public ExampleStoryCreateRequest MapToApplicationRequest(ExampleCreate.Request request)
    {
        return new ExampleStoryCreateRequest
        {
            Locale = request.Locale,
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }

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

    public ReadStoryRequest MapToApplicationRequest(Read.Request request)
    {
        return new ReadStoryRequest
        {
            StoryInfoId = new StoryInfoId(request.Id),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
    
    public Read.Response MapFromApplicationResponse(Domain.Aggregates.Story story)
    {
        return new Read.Response
        {
            StoryInfo = new StoryInfoDto
            {
                Id = story.StoryInfo.Id.Value,
                Name = story.StoryInfo.StoryName
            },
            Components = story.Components.Select(c => new ComponentDto
            {
                Branches = c.TraverseBranches().Select(MapToBranchDto),
                Fragments = c.TraverseFragments().Select(MapToFragmentDto)
            })
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