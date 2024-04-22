using OneOf;
using OneOf.Types;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.MarkEntrypoint;

public sealed class MarkFragmentEntrypointOneOf : OneOfBase<
    Success,
    Forbid,
    NotFound<StoryInfoVersionId>,
    NotFound<FragmentId>,
    ForbidEditBaseVersion>
{
    public MarkFragmentEntrypointOneOf(
        OneOf<Success, Forbid, NotFound<StoryInfoVersionId>, NotFound<FragmentId>, ForbidEditBaseVersion> input) : base(input)
    {
    }

    public static implicit operator MarkFragmentEntrypointOneOf(Success _) => new(_);
    public static implicit operator MarkFragmentEntrypointOneOf(Forbid _) => new(_);
    public static implicit operator MarkFragmentEntrypointOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator MarkFragmentEntrypointOneOf(NotFound<FragmentId> _) => new(_);
    public static implicit operator MarkFragmentEntrypointOneOf(ForbidEditBaseVersion _) => new(_);
}