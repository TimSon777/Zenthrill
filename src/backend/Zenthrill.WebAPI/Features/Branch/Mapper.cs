using Zenthrill.Application.Features.Branch;

namespace Zenthrill.WebAPI.Features.Branch;

public interface IMapper
{
    CreateBranchRequest MapToApplicationRequest(Create.Request request);
}

public sealed class Mapper : IMapper
{
    public CreateBranchRequest MapToApplicationRequest(Create.Request request)
    {
        return new CreateBranchRequest
        {
            Inscription = request.Inscription,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            },
            FromFragmentId = new FragmentId(request.FromFragmentId),
            ToFragmentId = new FragmentId(request.ToFragmentId)
        };
    }
}