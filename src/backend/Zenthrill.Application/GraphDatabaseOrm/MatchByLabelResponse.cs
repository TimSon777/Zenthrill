namespace Zenthrill.Application.GraphDatabaseOrm;

public sealed class MatchByLabelResponse<TNode, TBranch>
{
    public required List<TNode> Nodes { get; set; }

    public required List<TBranch> Relationships { get; set; }
}