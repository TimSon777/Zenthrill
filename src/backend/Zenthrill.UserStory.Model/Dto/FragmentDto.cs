namespace Zenthrill.UserStory.Model.Dto;

public sealed class FragmentDto
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Body { get; set; }
}