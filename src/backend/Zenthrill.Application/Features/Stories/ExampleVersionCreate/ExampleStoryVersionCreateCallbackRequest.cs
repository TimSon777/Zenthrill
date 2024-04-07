using TypesafeLocalization;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleVersionCreate;

public sealed class ExampleStoryVersionCreateCallbackRequest
{
    public required StoryInfoVersionId StoryInfoVersionId { get; set; }

    public required Locale Locale { get; set; }
}