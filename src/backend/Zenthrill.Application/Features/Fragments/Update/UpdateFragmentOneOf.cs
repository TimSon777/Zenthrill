using OneOf;
using OneOf.Types;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Update;

public sealed class UpdateFragmentOneOf : OneOfBase<
    Success,
    ValidationFailure,
    Forbid,
    NotFound<StoryInfoVersionId>,
    NotFound<FragmentId>,
    ForbidEditBaseVersion>
{
    public UpdateFragmentOneOf(
        OneOf<Success, ValidationFailure, Forbid, NotFound<StoryInfoVersionId>, NotFound<FragmentId>, ForbidEditBaseVersion> input) : base(input)
    {
    }

    public static implicit operator UpdateFragmentOneOf(Success _) => new(_);
    public static implicit operator UpdateFragmentOneOf(ValidationFailure _) => new(_);
    public static implicit operator UpdateFragmentOneOf(Forbid _) => new(_);
    public static implicit operator UpdateFragmentOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator UpdateFragmentOneOf(NotFound<FragmentId> _) => new(_);
    public static implicit operator UpdateFragmentOneOf(ForbidEditBaseVersion _) => new(_);
}