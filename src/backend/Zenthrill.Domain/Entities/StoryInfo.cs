using StronglyTypedIds;
using Zenthrill.Domain.Common;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Specifications;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct StoryInfoId;

public sealed class StoryInfo : Entity<StoryInfoId>
{
    public UserId CreatorId { get; set; }

    public ICollection<StoryInfoVersion> Versions { get; set; } = default!;

    public required string Description { get; set; }

    public List<Tag> Tags { get; set; } = default!;

    public StoryInfo()
    {
        Id = StoryInfoId.New();
    }

    public static Specification<StoryInfo> ById(StoryInfoId storyInfoId)
    {
        return new Specification<StoryInfo>(storyInfo => storyInfo.Id == storyInfoId);
    }
}
