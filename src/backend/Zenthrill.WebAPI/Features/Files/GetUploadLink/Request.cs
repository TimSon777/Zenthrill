using Microsoft.AspNetCore.Mvc;

namespace Zenthrill.WebAPI.Features.Files.GetUploadLink;

public sealed class Request
{
    [FromQuery(Name = "storyInfoId")]
    public required Guid StoryInfoId { get; set; }

    [FromQuery(Name = "fileName")]
    public required string FileName { get; set; }
}