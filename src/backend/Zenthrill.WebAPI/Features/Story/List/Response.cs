using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Story.List;

public sealed class Response
{
    public required IEnumerable<StoryInfoDto> StoryInfos { get; set; }
}