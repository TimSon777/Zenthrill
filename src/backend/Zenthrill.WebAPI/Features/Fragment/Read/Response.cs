using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Fragment.Read;

public sealed class Response
{
    public required FragmentDto Fragment { get; set; }

    public required IEnumerable<BranchDto> OutputBranches { get; set; }
}