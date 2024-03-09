using Zenthrill.Application.Features.Branches.Create;
using Zenthrill.Application.Features.Branches.Update;
using Zenthrill.WebAPI.Features.Branch.Update;

namespace Zenthrill.WebAPI.Features.Branch;

public interface IMapper
{
    CreateBranchRequest MapToApplicationRequest(Create.Request request);
    
    UpdateBranchRequest MapToApplicationRequest(Update.Request request);
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

    public UpdateBranchRequest MapToApplicationRequest(Request request)
    {
        return new UpdateBranchRequest
        {
            BranchId = new BranchId(request.BranchId),
            Inscription = request.Inscription,
            StoryInfoId = new StoryInfoId(request.StoryInfoId),
            User = new User
            {
                Nickname = "Test",
                Id = new UserId(new Guid("cffc1c0c-3a86-42dc-94c0-5e9a0c6ab5a6"))
            }
        };
    }
}