using Zenthrill.Domain.Aggregates;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Repositories;

public interface IStoryRepository
{
    Task<Story> ReadAsync(StoryInfo storyInfo, CancellationToken cancellationToken);
}