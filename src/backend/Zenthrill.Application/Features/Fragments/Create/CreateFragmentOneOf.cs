using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Create;

public sealed class CreateFragmentOneOf : OneOfBase<
    FragmentId,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoVersionId>,
    NotFound<FragmentId>,
    ForbidEditBaseVersion>
{
    public CreateFragmentOneOf(OneOf<FragmentId, ValidationFailure, Forbid, NotFound<StoryInfoVersionId>, NotFound<FragmentId>, ForbidEditBaseVersion> input) : base(input)
    {
    }
    
    public static implicit operator CreateFragmentOneOf(FragmentId _) => new(_);
    public static implicit operator CreateFragmentOneOf(ValidationFailure _) => new(_);
    public static implicit operator CreateFragmentOneOf(Forbid _) => new(_);
    public static implicit operator CreateFragmentOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator CreateFragmentOneOf(NotFound<FragmentId> _) => new(_);
    public static implicit operator CreateFragmentOneOf(ForbidEditBaseVersion _) => new(_);
}