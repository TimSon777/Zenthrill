using Zenthrill.Application.Features.Stories.CreateVersion.Objects;

namespace Zenthrill.WebAPI.Objects;

public sealed class StoryVersionDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
    
    public required VersionDto Version { get; set; }
}