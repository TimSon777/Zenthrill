using Zenthrill.WebAPI.Features.Story.CreateVersion.Objects;

namespace Zenthrill.WebAPI.Features.Story.CreateVersion;

public sealed class Request
{
    public required Guid BaseStoryInfoVersionId { get; set; }
    public required string Name { get; set; }

    public required StoryVersionDto Version { get; set; }
}