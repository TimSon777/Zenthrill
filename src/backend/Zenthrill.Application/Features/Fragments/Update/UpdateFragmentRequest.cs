using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Update;

public sealed class UpdateFragmentRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; init; }
    
    public required FragmentId FragmentId { get; init; }
    
    public required string Body { get; init; }
    
    public required User User { get; init; }
    
    public required string Name { get; set; }
}