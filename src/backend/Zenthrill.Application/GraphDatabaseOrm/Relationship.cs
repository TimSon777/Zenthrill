namespace Zenthrill.Application.GraphDatabaseOrm;

public abstract class Relationship
{
    public IReadOnlyCollection<string> Labels { get; set; } = default!;
}