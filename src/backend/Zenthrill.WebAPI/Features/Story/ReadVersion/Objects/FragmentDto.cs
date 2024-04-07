namespace Zenthrill.WebAPI.Features.Story.ReadVersion.Objects;

public sealed class FragmentDto
{
    public required Guid Id { get; set; }

    public required bool IsEntrypoint { get; set; }

    public required string Body { get; set; }
}