using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct TagId;

public sealed class Tag : Entity<TagId>
{
    public required string Name { get; set; }
    
    public ICollection<StoryInfo> StoryInfos { get; set; } = default!;
}