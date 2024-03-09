using TypesafeLocalization;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.ExampleCreate;

public sealed class ExampleStoryCreateRequest
{
    public required User User { get; set; }

    public required Locale Locale { get; set; }
}