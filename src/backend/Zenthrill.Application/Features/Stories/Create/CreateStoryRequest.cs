using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Features.Stories.Create;

public sealed class CreateStoryRequest
{
    public required User User { get; init; }

    public required string Description { get; init; }
}