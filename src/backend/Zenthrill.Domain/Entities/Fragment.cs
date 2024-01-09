using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

public sealed class FragmentId : HasId<Guid>;

public sealed class Fragment : Entity<FragmentId>
{
    public required string Body { get; set; }

    public required List<Branch> Branches { get; set; }
}

public sealed class EntrypointFragment : Entity<FragmentId>;