using Zenthrill.Application.Features.Stories.CreateVersion.Objects;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.CreateVersion;

public sealed class CreateStoryVersionRequest
{
   public required string Name { get; set; }
   
   public required StoryVersionDto Version { get; set; }
   
   public required User User { get; set; }

   public required StoryInfoVersionId BaseStoryInfoVersionId { get; set; }
}