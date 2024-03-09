using Zenthrill.Application.Extensions;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Domain.Aggregates;

public sealed class Story
{
    public required StoryInfo StoryInfo { get; init; }

    private readonly List<Fragment> _components = [];

    public IReadOnlyCollection<Fragment> Components => _components;

    public bool IsConnectedStory => Components.Count == 1;

    public bool AddComponent(Fragment fragment)
    {
        var fragments = fragment.TraverseFragments();

        if (_components.Any(c => fragments.Any(f => f == c)))
        {
            return false;
        }
        
        _components.Add(fragment);

        return true;
    }

    public HashSet<Fragment> TraverseFragments()
    {
        var result = new HashSet<Fragment>();
        foreach (var component in Components)
        {
            var fragments = component.TraverseFragments();
            result.AddRange(fragments);
        }

        return result;
    }

    public HashSet<Branch> TraverseBranches()
    {
        var result = new HashSet<Branch>();
        foreach (var component in Components)
        {
            var branches = component.TraverseBranches();
            result.AddRange(branches);
        }

        return result;
    }
}