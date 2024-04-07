using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public sealed class CreateStoryVersionOneOf : OneOfBase<
    StoryInfoVersionId,
    NotFound<StoryInfoVersionId>,
    ValidationFailure,
    Forbid>
{
    public CreateStoryVersionOneOf(
        OneOf<StoryInfoVersionId, NotFound<StoryInfoVersionId>, ValidationFailure, Forbid> input) : base(input)
    {
    }

    public static implicit operator CreateStoryVersionOneOf(StoryInfoVersionId _) => new(_);
    public static implicit operator CreateStoryVersionOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator CreateStoryVersionOneOf(ValidationFailure _) => new(_);
    public static implicit operator CreateStoryVersionOneOf(Forbid _) => new(_);
}