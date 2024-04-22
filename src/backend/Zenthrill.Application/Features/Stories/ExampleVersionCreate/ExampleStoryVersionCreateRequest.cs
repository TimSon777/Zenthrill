using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleVersionCreate;

public sealed class ExampleStoryVersionCreateRequest
{
    public required User User { get; set; }
    
    public required StoryInfoId StoryInfoId { get; set; }
}