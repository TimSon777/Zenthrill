namespace Zenthrill.Application.Repositories;

public interface IRepositoryRegistry
{
    IFragmentRepository FragmentRepository { get; }
    
    IBranchRepository BranchRepository { get; }
    
    IStoryRepository StoryRepository { get; }
}
