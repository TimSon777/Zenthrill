namespace Zenthrill.WebAPI.Features.Fragment.Create;

public sealed class Request
{
    public required Guid StoryInfoVersionId { get; set; }

    public required string Body { get; set; } = default!;
}