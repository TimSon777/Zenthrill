using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Story.Read;

public sealed class Response
{
    public required StoryInfoDto StoryInfo { get; set; }

    public required IEnumerable<StoryVersionDto> Versions { get; set; }
}