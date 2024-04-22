namespace Zenthrill.WebAPI.Features.Fragment.MarkEntrypoint;

public sealed class Request
{
    public required Guid Id { get; set; }
    
    public required Guid StoryInfoVersionId { get; set; }
}