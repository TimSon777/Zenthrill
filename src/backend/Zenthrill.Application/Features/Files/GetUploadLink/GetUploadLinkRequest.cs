using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Files.GetUploadLink;

public sealed class GetUploadLinkRequest
{
    public required StoryInfoId StoryInfoId { get; set; }
    
    public required User User { get; set; }

    public required string FileName { get; set; }
}