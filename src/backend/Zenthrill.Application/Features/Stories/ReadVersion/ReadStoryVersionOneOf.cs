using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ReadVersion;

public sealed class ReadStoryVersionOneOf : OneOfBase<
    Story,
    NotFound<StoryInfoVersionId>,
    Forbid>
{
    public ReadStoryVersionOneOf(
        OneOf<Story, NotFound<StoryInfoVersionId>, Forbid> input) : base(input)
    {
    }

    public static implicit operator ReadStoryVersionOneOf(Story _) => new(_);
    public static implicit operator ReadStoryVersionOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator ReadStoryVersionOneOf(Forbid _) => new(_);
}