using Zenthrill.UserStory.Model.Models;

namespace Zenthrill.UserStory.Model.Entities;

public sealed class Story(Guid id)
{
    public Guid Id { get; set; } = id;

    public Story() : this(Guid.NewGuid())
    {
    }
    
    public required Guid StoryInfoVersionId { get; set; }
    
    public required StoryExecutionContext ExecutionContext { get; set; }
    
    public required User User { get; set; }
}