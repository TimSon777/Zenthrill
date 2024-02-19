using Zenthrill.WebAPI.Features.Story.Read.Objects;

namespace Zenthrill.WebAPI.Features.Story.Read;

public sealed class Response
{
    public required FragmentDto? Entrypoint { get; set; }

    public required List<FragmentDto> Fragments { get; set; }
    
    public required List<BranchDto> Branches { get; set; }
}