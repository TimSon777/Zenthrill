namespace Zenthrill.UserStory.Model.Models;

public sealed class Fragment
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Body { get; set; }
}