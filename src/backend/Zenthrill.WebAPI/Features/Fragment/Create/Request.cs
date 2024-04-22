namespace Zenthrill.WebAPI.Features.Fragment.Create;

public sealed class Request
{
    public required Guid StoryInfoVersionId { get; set; }

    public required string Body { get; set; } = default!;

    public required Guid? FromFragmentId { get; set; }
    
    public required string Name { get; set; }
}