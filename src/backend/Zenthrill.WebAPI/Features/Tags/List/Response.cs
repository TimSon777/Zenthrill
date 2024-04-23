using Zenthrill.WebAPI.Objects;

namespace Zenthrill.WebAPI.Features.Tags.List;

public sealed class Response
{
    public required IEnumerable<TagDto> Tags { get; set; }   
}