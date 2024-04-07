using TypesafeLocalization;

namespace Zenthrill.WebAPI.Features.Story.ExampleVersionCreate;

public sealed class Request
{
    public required Locale Locale { get; set; }

    public required Guid StoryInfoId { get; set; }
}