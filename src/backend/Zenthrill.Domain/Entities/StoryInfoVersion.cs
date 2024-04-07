using StronglyTypedIds;
using Zenthrill.Domain.Common;
using Zenthrill.Domain.ValueObjects;
using Zenthrill.Specifications;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct StoryInfoVersionId;

public sealed class StoryInfoVersion : Entity<StoryInfoVersionId>
{
    public StoryInfoId StoryInfoId { get; set; }

    public StoryInfo StoryInfo { get; set; } = default!;

    public FragmentId? EntrypointFragmentId { get; set; }

    public string Name { get; set; } = default!;

    public StoryVersion Version { get; set; } = default!;

    public StoryInfoVersionId? BaseVersionId { get; set; }

    public StoryInfoVersion? BaseVersion { get; set; }

    public ICollection<StoryInfoVersion> SubVersions { get; set; } = default!;

    public bool IsBaseVersion => SubVersions.Any();

    public StoryInfoVersion()
    {
        Id = StoryInfoVersionId.New();
    }

    public static Specification<StoryInfoVersion> ById(StoryInfoVersionId id)
    {
        return new Specification<StoryInfoVersion>(storyInfoVersion => storyInfoVersion.Id == id);
    }
}
