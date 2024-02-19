using Zenthrill.Application.GraphDatabaseOrm;
using Zenthrill.Application.Results;
using Zenthrill.Domain.ValueObjects;
using Success = OneOf.Types.Success;

namespace Zenthrill.Application.Services;

public interface IGraphDatabase
{
    Task<OneOf.OneOf<Success, DatabaseAlreadyExists>> CreateGraphAsync(string name);

    Task<OneOf.OneOf<Id<long>>> CreateNodeAsync<TValue>(string database, TValue value, IEnumerable<string> labels)
        where TValue : notnull;

    Task<List<Id<long>>> CreateRelationshipAsync<TLeftValue, TValue, TRightValue>(
        string database,
        TLeftValue leftMatch,
        TRightValue rightMatch,
        TValue value,
        IEnumerable<string> labels);

    Task<(TValue? Value, IEnumerable<string> Labels)> FindNodeAsync<TValue, TMatch>(string database, TMatch match)
        where TValue : notnull;

    Task<MatchByLabelResponse<TNode, TBranch>> GetNodesAndRelationshipsAsync<TNode, TBranch>(
        string database,
        string label)
        where TNode : notnull
        where TBranch : notnull;
}