namespace Zenthrill.WebAPI.Features.Fragment.Update;

public sealed class Request
{
    public required Guid StoryInfoId { get; set; }
    
    public required Guid FragmentInfoId { get; set; }

    public required string Body { get; set; } = default!;
}