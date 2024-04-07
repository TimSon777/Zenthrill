using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public sealed class CreateStoryVersionCallbackRequest
{
    public StoryInfoVersionId StoryInfoVersionId { get; set; }
}