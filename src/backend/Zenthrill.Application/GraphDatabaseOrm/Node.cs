namespace Zenthrill.Application.GraphDatabaseOrm;

public abstract class Node
{
    public IReadOnlyCollection<string> Labels { get; set; } = default!;
}