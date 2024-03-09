using TypesafeLocalization;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleCreate;

public sealed class ExampleStoryCreateCallbackRequest
{
    public required StoryInfoId StoryInfoId { get; set; }

    public required Locale Locale { get; set; }
}