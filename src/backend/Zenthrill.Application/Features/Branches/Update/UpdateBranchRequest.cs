using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Branches.Update;

public sealed class UpdateBranchRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; init; }

    public required BranchId BranchId { get; set; }

    public required string Inscription { get; init; }

    public required User User { get; init; }
}