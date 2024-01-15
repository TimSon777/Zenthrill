using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct FragmentId;

public sealed class Fragment : Entity<FragmentId>
{
    public required string Body { get; set; }

    public required List<Branch> Branches { get; set; }

    public Fragment()
    {
        Id = FragmentId.New();
    }
}

public sealed class EntrypointFragment : Entity<FragmentId>;