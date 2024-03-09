using TypesafeLocalization;

namespace Zenthrill.WebAPI.Features.Story.ExampleCreate;

public sealed class Request
{
    public required Locale Locale { get; set; }
}