using Zenthrill.Application.Results;
using Zenthrill.Domain.ValueObjects;
using Success = OneOf.Types.Success;

namespace Zenthrill.Application.Services;


public interface IGraphDatabase
{
    Task<OneOf.OneOf<Success, DatabaseAlreadyExists>> CreateGraphAsync(string name);

    Task<OneOf.OneOf<Id<long>>> CreateNodeAsync<TValue>(string database, TValue value, IEnumerable<string> labels)
        where TValue : notnull;

    Task CreateRelationshipAsync<TLeftValue, TValue, TRightValue>(
        string database,
        TLeftValue leftMatch,
        TRightValue rightMatch,
        TValue value,
        IEnumerable<string> labels);

    Task<(TValue Value, IEnumerable<string> Labels)> FindNodeAsync<TValue, TMatch>(string database, TMatch match);
}