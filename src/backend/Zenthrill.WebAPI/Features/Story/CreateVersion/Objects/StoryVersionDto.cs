namespace Zenthrill.WebAPI.Features.Story.CreateVersion.Objects;

public sealed class StoryVersionDto
{
    public required int Major { get; set; }

    public required int Minor { get; set; }

    public required string Suffix { get; set; }
}