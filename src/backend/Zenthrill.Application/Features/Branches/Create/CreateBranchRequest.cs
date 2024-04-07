using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Create;

public sealed class CreateBranchRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; init; }

    public required FragmentId FromFragmentId { get; init; }
    
    public required FragmentId ToFragmentId { get; init; }
    
    public required string Inscription { get; init; }

    public required User User { get; init; }
}