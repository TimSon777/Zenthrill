using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ReadVersion;

public sealed class ReadStoryVersionRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; set; }

    public required User User { get; set; }
}