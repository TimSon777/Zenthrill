using StronglyTypedIds;
using Zenthrill.Domain.Common;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct StoryInfoId;

public sealed class StoryInfo : Entity<StoryInfoId>
{
    public required string StoryName { get; set; }

    public required FragmentId EntrypointFragmentId { get; set; }

    public required StoryVersion Version { get; set; }

    public UserId CreatorId { get; set; }
    
    public User Creator { get; set; } = default!;

    public StoryInfo()
    {
        Id = StoryInfoId.New();
    }
}
