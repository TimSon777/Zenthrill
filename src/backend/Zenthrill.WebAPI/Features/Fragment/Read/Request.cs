using Microsoft.AspNetCore.Mvc;

namespace Zenthrill.WebAPI.Features.Fragment.Read;

public sealed class Request
{
    [FromRoute(Name = "fragmentId")]
    public required Guid Id { get; set; }

    [FromRoute(Name = "storyInfoVersionId")]
    public required Guid StoryInfoVersionId { get; set; }
}