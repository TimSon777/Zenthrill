using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Read;

public sealed class ReadFragmentRequest
{
    public required FragmentId Id { get; set; }

    public required StoryInfoVersionId StoryInfoVersionId { get; set; }
}