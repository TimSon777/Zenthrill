namespace Zenthrill.WebAPI.Features.Story.Create;

public sealed class Request
{
    public required string Description { get; set; } = default!;
}