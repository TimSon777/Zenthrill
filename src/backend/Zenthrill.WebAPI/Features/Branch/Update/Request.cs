namespace Zenthrill.WebAPI.Features.Branch.Update;

public sealed class Request
{
    public required Guid StoryInfoVersionId { get; set; }

    public required string Inscription { get; set; }

    public required Guid BranchId { get; set; }
}