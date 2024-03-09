using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Update;

public sealed class UpdateBranchOneOf : OneOfBase<
    BranchId,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoId>,
    NotFound<BranchId>>
{
    public UpdateBranchOneOf(OneOf<BranchId, ValidationFailure, Forbid, NotFound<StoryInfoId>, NotFound<BranchId>> input)
        : base(input)
    {
    }
    
    public static implicit operator UpdateBranchOneOf(BranchId _) => new(_);
    public static implicit operator UpdateBranchOneOf(ValidationFailure _) => new(_);
    public static implicit operator UpdateBranchOneOf(Forbid _) => new(_);
    public static implicit operator UpdateBranchOneOf(NotFound<StoryInfoId> _) => new(_);
    public static implicit operator UpdateBranchOneOf(NotFound<BranchId> _) => new(_);
}