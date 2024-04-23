using Zenthrill.Domain.Entities;

namespace Zenthrill.Domain.Aggregates;

public sealed class Story
{
    public required StoryInfoId StoryInfoId { get; set; }
    
    public required string Description { get; set; }
    
    public required IReadOnlyCollection<StoryInfoVersion> Versions { get; set; }
    
    public required IReadOnlyCollection<Tag> Tags { get; set; }
}