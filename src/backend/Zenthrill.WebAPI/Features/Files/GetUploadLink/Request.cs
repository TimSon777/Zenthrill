namespace Zenthrill.WebAPI.Features.Files.GetUploadLink;

public sealed class Request
{
    public required Guid StoryInfoId { get; set; }

    public required string Extension { get; set; }
}