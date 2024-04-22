namespace Zenthrill.WebAPI.Features.Fragment.Update;

public sealed class Request
{
    public required Guid StoryInfoVersionId { get; set; }
    
    public required Guid FragmentId { get; set; }

    public required string Body { get; set; } = default!;

    public required string Name { get; set; }
}