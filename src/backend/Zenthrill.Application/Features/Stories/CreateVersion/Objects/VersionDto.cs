namespace Zenthrill.Application.Features.Stories.CreateVersion.Objects;

public sealed class VersionDto
{
    public required int Major { get; set; }
    
    public required int Minor { get; set; }
    
    public required string Suffix { get; set; }
}