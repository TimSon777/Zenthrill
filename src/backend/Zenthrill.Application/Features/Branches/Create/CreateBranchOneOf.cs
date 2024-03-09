using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Create;

public sealed class CreateBranchOneOf : OneOfBase<
    BranchId,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoId>,
    FragmentDoesNotExist>
{
    public CreateBranchOneOf(OneOf<BranchId, ValidationFailure, Forbid, NotFound<StoryInfoId>, FragmentDoesNotExist> input) : base(input)
    {
    }
    
    public static implicit operator CreateBranchOneOf(BranchId _) => new(_);
    public static implicit operator CreateBranchOneOf(ValidationFailure _) => new(_);
    public static implicit operator CreateBranchOneOf(Forbid _) => new(_);
    public static implicit operator CreateBranchOneOf(NotFound<StoryInfoId> _) => new(_);
    public static implicit operator CreateBranchOneOf(FragmentDoesNotExist _) => new(_);

}