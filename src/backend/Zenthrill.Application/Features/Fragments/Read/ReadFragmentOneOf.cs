using OneOf;
using Zenthrill.Application.Results;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Read;

public sealed class ReadFragmentOneOf(
    OneOf<Fragment, NotFound<StoryInfoVersionId>, NotFound<FragmentId>> input)
    : OneOfBase<
        Fragment,
        NotFound<StoryInfoVersionId>,
        NotFound<FragmentId>>(input)
{
    public static implicit operator ReadFragmentOneOf(Fragment _) => new(_);
    public static implicit operator ReadFragmentOneOf(NotFound<StoryInfoVersionId> _) => new(_);
    public static implicit operator ReadFragmentOneOf(NotFound<FragmentId> _) => new(_);
}