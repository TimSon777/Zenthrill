using Zenthrill.UserStory.Model.Dto;

namespace Zenthrill.UserStory.Model.Clients.Dto;

public sealed class FragmentResponse
{
    public required ReadFragmentValue Value { get; set; }
}

public sealed class ReadFragmentValue
{
    public required FragmentDto Fragment { get; set; }

    public required IEnumerable<BranchDto> OutputBranches { get; set; }
}