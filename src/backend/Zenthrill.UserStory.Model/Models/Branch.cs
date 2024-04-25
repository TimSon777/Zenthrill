namespace Zenthrill.UserStory.Model.Models;

public sealed class Branch
{
    public required Guid Id { get; set; }
    
    public required string Inscription { get; set; }
    
    public required Guid FromFragmentId { get; set; }
    
    public required Guid ToFragmentId { get; set; }
}