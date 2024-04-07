using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Create;

public sealed class CreateBranchOneOf : OneOfBase<
    BranchId,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoVersionId>,
    ForbidEditBaseVersion,
    FragmentDoesNotExist>
{
    public CreateBranchOneOf(OneOf<BranchId, ValidationFailure, Forbid, NotFound<StoryInfoVersionId>, ForbidEditBaseVersion, FragmentDoesNotExist> input) : base(input)
    {
    }
    
    public static implicit operator CreateBranchOneOf(BranchId _) => new(_);
    public static implicit operator CreateBranchOneOf(ValidationFailure _) => new(_);
    public static implicit operator CreateBranchOneOf(Forbid _) => new(_);
    public static implicit operator CreateBranchOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator CreateBranchOneOf(ForbidEditBaseVersion _) => new(_);
    public static implicit operator CreateBranchOneOf(FragmentDoesNotExist _) => new(_);

}