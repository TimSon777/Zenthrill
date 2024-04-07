using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Fragments.Create;

public sealed class CreateFragmentRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; init; }
    
    public required string Body { get; init; }
    
    public required User User { get; init; }
}