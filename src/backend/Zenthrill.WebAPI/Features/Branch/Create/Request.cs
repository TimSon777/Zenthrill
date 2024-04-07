namespace Zenthrill.WebAPI.Features.Branch.Create;

public sealed class Request
{
    public required Guid StoryInfoVersionId { get; set; }

    public required Guid FromFragmentId { get; set; }
    
    public required Guid ToFragmentId { get; set; }
    
    public required string Inscription { get; set; }
}