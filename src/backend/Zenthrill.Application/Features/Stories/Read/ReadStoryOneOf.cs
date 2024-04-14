using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.Read;

public sealed class ReadStoryOneOf : OneOfBase<
    Story,
    NotFound<StoryInfoId>,
    Forbid>
{
    public ReadStoryOneOf(
        OneOf<Story, NotFound<StoryInfoId>, Forbid> input) : base(input)
    {
    }

    public static implicit operator ReadStoryOneOf(Story _) => new(_);
    public static implicit operator ReadStoryOneOf(NotFound<StoryInfoId> _) => new(_);
    public static implicit operator ReadStoryOneOf(Forbid _) => new(_);
}