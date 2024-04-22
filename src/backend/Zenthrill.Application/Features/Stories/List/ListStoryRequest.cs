using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.List;

public sealed class ListStoryRequest
{
    public required User User { get; set; }
}