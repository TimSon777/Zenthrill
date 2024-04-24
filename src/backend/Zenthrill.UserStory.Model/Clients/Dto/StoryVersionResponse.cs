using Zenthrill.UserStory.Model.Dto;

namespace Zenthrill.UserStory.Model.Clients.Dto;

public sealed class StoryVersionResponse
{
    public required Value Value { get; set; }
}

public sealed class Value
{
    public required Guid? EntrypointFragmentId { get; set; }
    
    public required IEnumerable<ComponentDto> Components { get; set; }
    public required string Name { get; set; }
}

public sealed class ComponentDto
{
    public required IEnumerable<FragmentDto> Fragments { get; set; }

    public required IEnumerable<BranchDto> Branches { get; set; }
}