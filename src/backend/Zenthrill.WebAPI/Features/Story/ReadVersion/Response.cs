using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Story.ReadVersion;

public sealed class Response
{
    public required StoryInfoDto StoryInfo { get; set; }

    public required IEnumerable<ComponentDto> Components { get; set; }

    public required string Name { get; set; }
    
    public required VersionDto Version { get; set; }
    
    public required Guid Id { get; set; }
    
    public required Guid? EntrypointFragmentId { get; set; }

    public required bool IsPublished { get; set; }
    
    public required Guid? EntrypointId { get; set; }
}