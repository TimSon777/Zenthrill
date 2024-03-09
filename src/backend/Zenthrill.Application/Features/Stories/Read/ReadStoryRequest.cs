using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.Read;

public sealed class ReadStoryRequest
{
    public required StoryInfoId StoryInfoId { get; set; }

    public required User User { get; set; }
}