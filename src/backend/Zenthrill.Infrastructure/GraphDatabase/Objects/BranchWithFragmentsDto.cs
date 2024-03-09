using Zenthrill.Infrastructure.Repositories;

namespace Zenthrill.Infrastructure.GraphDatabase.Objects;

public sealed class BranchWithFragmentsDto
{
    public required FragmentDto FromFragment { get; set; }

    public required FragmentDto ToFragment { get; set; }

    public required BranchDto Branch { get; set; }
}