using System.Security.Claims;
using Zenthrill.Application.Features.Branches.Create;
using Zenthrill.Application.Features.Branches.Update;
using Zenthrill.WebAPI.Common;

namespace Zenthrill.WebAPI.Features.Branch;

public interface IMapper
{
    CreateBranchRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal);
    
    UpdateBranchRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal);
}

public sealed class Mapper(IUserMapper userMapper) : IMapper
{
    public CreateBranchRequest MapToApplicationRequest(Create.Request request, ClaimsPrincipal principal)
    {
        return new CreateBranchRequest
        {
            Inscription = request.Inscription,
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            User = userMapper.MapToApplicationUser(principal),
            FromFragmentId = new FragmentId(request.FromFragmentId),
            ToFragmentId = new FragmentId(request.ToFragmentId)
        };
    }

    public UpdateBranchRequest MapToApplicationRequest(Update.Request request, ClaimsPrincipal principal)
    {
        return new UpdateBranchRequest
        {
            BranchId = new BranchId(request.BranchId),
            Inscription = request.Inscription,
            StoryInfoVersionId = new StoryInfoVersionId(request.StoryInfoVersionId),
            User = userMapper.MapToApplicationUser(principal)
        };
    }
}