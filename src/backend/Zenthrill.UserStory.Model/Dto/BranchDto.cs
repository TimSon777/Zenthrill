namespace Zenthrill.UserStory.Model.Dto;

public sealed class BranchDto
{
    public required Guid Id { get; set; }
    
    public required string Inscription { get; set; }
    
    public required Guid FromFragmentId { get; set; }
    
    public required Guid ToFragmentId { get; set; }
}