namespace Zenthrill.WebAPI.Features.Story.ReadVersion.Objects;

public sealed class StoryInfoDto
{
    public required Guid Id { get; set; }

    public required string Description { get; set; }
}