namespace Zenthrill.WebAPI.Objects;

public sealed class ComponentDto
{
    public required IEnumerable<FragmentDto> Fragments { get; set; }

    public required IEnumerable<BranchDto> Branches { get; set; }
}