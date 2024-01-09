using Zenthrill.Domain.Common;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Domain.Entities;

public sealed class StoryInfoId : HasId<Guid>;

public sealed class StoryInfo : Entity<StoryInfoId>
{
    public required string StoryName { get; set; }

    public required FragmentId EntrypointFragmentId { get; set; }

    public required StoryVersion Version { get; set; }

    public UserId CreatorId { get; set; } = default!;
    
    public User Creator { get; set; } = default!;

    public StoryInfo()
    {
        Id = new StoryInfoId
        {
            Id = Guid.NewGuid()
        };
    }
}