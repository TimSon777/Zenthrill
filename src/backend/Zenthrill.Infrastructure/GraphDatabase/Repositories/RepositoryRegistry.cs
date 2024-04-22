using Zenthrill.Application.Repositories;

namespace Zenthrill.Infrastructure.GraphDatabase.Repositories;

public sealed class RepositoryRegistry(
    IFragmentRepository fragmentRepository,
    IBranchRepository branchRepository,
    IStoryRepository storyRepository) : IRepositoryRegistry
{
    public IFragmentRepository FragmentRepository { get; } = fragmentRepository;

    public IBranchRepository BranchRepository { get; } = branchRepository;

    public IStoryRepository StoryRepository { get; } = storyRepository;
}