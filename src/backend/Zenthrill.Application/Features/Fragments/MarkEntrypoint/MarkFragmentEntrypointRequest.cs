using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.MarkEntrypoint;

public sealed class MarkFragmentEntrypointRequest
{
    public required FragmentId Id { get; set; }

    public required StoryInfoVersionId StoryInfoVersionId { get; set; }
    
    public required User User { get; set; }
}