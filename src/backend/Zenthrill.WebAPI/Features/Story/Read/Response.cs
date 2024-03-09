using Zenthrill.WebAPI.Features.Story.Read.Objects;

namespace Zenthrill.WebAPI.Features.Story.Read;

public sealed class Response
{
    public required StoryInfoDto StoryInfo { get; set; }

    public required IEnumerable<ComponentDto> Components { get; set; }
}