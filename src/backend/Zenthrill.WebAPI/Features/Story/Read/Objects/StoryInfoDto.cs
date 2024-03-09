namespace Zenthrill.WebAPI.Features.Story.Read.Objects;

public sealed class StoryInfoDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
}