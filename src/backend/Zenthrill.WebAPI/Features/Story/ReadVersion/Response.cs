using Zenthrill.WebAPI.Features.Story.ReadVersion.Objects;

namespace Zenthrill.WebAPI.Features.Story.ReadVersion;

public sealed class Response
{
    public required StoryInfoDto StoryInfo { get; set; }

    public required IEnumerable<ComponentDto> Components { get; set; }

    public required string Name { get; set; }
}