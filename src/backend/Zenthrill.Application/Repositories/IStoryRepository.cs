using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IStoryRepository
{
    Task<StoryVersion> ReadAsync(StoryInfoVersion storyInfoVersion, CancellationToken cancellationToken);
}