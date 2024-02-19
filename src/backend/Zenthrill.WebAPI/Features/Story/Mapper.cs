using Zenthrill.Application.Features.Story;
using Zenthrill.WebAPI.Features.Story.Read.Objects;

namespace Zenthrill.WebAPI.Features.Story;

public interface IMapper
{
    CreateStoryRequest MapToApplicationRequest(Create.Request request);
    
    ReadStoryRequest MapToApplicationRequest(Read.Request request);

    Read.Response MapFromApplicationResponse(ReadStoryResponse response);
}

public sealed class Mapper : IMapper
{
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
    
    public Read.Response MapFromApplicationResponse(ReadStoryResponse response)
    {
        return new Read.Response
        {
            Entrypoint = response.Entrypoint is not null
                ? MapToFragmentDto(response.Entrypoint)
                : null,
            Fragments = response.Fragments.ConvertAll(MapToFragmentDto),
            Branches = response.Branches.ConvertAll(branch => new BranchDto
            {
                Id = branch.Id.Value,
                Inscription = branch.Inscription,
                FromFragmentId = branch.FromFragmentId.Value,
                ToFragmentId = branch.ToFragmentId.Value
            })
        };
    }

    private static FragmentDto MapToFragmentDto(ReadStoryFragment fragment)
    {
        return new FragmentDto
        {
            Id = fragment.Id.Value,
            Body = fragment.Body
        };
    }
}