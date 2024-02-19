using System.Collections.ObjectModel;
using Neo4j.Driver;
using OneOf.Types;
using Zenthrill.Application.Extensions;
using Zenthrill.Application.GraphDatabaseOrm;
using Zenthrill.Application.Results;
using Zenthrill.Application.Services;
using Zenthrill.Domain.ValueObjects;

namespace Zenthrill.Infrastructure.Services;

public sealed class GraphDatabase(IGraphDatabaseDriver graphDatabaseDriver) : IGraphDatabase
{
    public async Task<OneOf.OneOf<Success, DatabaseAlreadyExists>> CreateGraphAsync(string name)
    {
        await using var session = graphDatabaseDriver.AsyncSession(builder =>
            builder.WithDatabase("system"));

        try
        {
            await session.RunAsync($"CREATE DATABASE `{name}`");
        }
        catch (ClientException ex)
            when(ex.Message.Contains("Database name or alias already exists"))
        {
            return new DatabaseAlreadyExists();
        }

        return new Success();
    }

    public async Task<OneOf.OneOf<Id<long>>> CreateNodeAsync<TValue>(string database, TValue value, IEnumerable<string> labels)
        where TValue : notnull
    {
        await using var session = graphDatabaseDriver.AsyncSession(builder =>
            builder.WithDatabase(database));

        var properties = GetProperties(value);

        var convertedLabels = string.Join(":", labels);

        var query = $"CREATE (n:{convertedLabels} $props) RETURN id(n)";

        var result = await session.ExecuteWriteAsync(async runner =>
        {
            var cursor = await runner.RunAsync(query, new
            {
                props = properties
            });

            return await cursor.SingleAsync();
        });

        return Id.Create(result[0].As<long>());
    }

    public async Task<List<Id<long>>> CreateRelationshipAsync<TLeftValue, TValue, TRightValue>(
        string database,
        TLeftValue leftMatch,
        TRightValue rightMatch,
        TValue value,
        IEnumerable<string> labels)
    {
        await using var session = graphDatabaseDriver.AsyncSession(builder =>
            builder.WithDatabase(database));

        var leftProperties = ConvertToStringProperties(leftMatch);
        var rightProperties = ConvertToStringProperties(rightMatch);
        var properties = GetProperties(value);

        var convertedLabels = string.Join(":", labels);

        var query = $"""
                     MATCH (a {leftProperties}), (b {rightProperties})
                     CREATE (a)-[:{convertedLabels} $props]->(b)
                     RETURN id(a), id(b)
                     """;

        var result = await session.ExecuteWriteAsync(async runner =>
        {
            var cursor = await runner.RunAsync(query, new
            {
                props = properties
            });

            return await cursor.ToListAsync();
        });

        return result
            .Select(r => Id.Create(r[0].As<long>()))
            .ToList();
    }

    public async Task<(TValue? Value, IEnumerable<string> Labels)> FindNodeAsync<TValue, TMatch>(string database, TMatch match)
        where TValue : notnull
    {
        await using var session = graphDatabaseDriver.AsyncSession(builder =>
            builder.WithDatabase(database));

        var properties = ConvertToStringProperties(match);

        var query = $"""
                     MATCH (a {properties})
                     RETURN a
                     """;

        var record = await session.ExecuteReadAsync(async runner =>
        {
            var cursor = await runner.RunAsync(query);
            return await cursor.SingleAsync();
        });

        var node = (INode)record[0];
        var value = node.Properties.ToObject<TValue>();
        
        return (value, node.Labels);
    }

    public async Task<MatchByLabelResponse<TNode, TBranch>> GetNodesAndRelationshipsAsync<TNode, TBranch>(string database, string label) where TNode : notnull where TBranch : notnull
    {
        await using var session = graphDatabaseDriver.AsyncSession(builder =>
            builder.WithDatabase(database));

        var query = $"""
                     MATCH (node:{label}) -[r]- ()
                     RETURN DISTINCT node, collect(r) as relationship
                     """;

        var (nodes, relationships) = await session.ExecuteReadAsync(async runner =>
        {
            var cursor = await runner.RunAsync(query);
            var list = await cursor.ToListAsync();

            var ormNodes = list
                .SelectMany(e => e.Values
                    .Where(v => v.Key == "node")
                    .Select(v => v.Value))
                .Cast<INode>()
                .ToList();
            
            var dictNodes = ormNodes
                .Select(v =>
                {
                    var node = v.Properties.ToObject<TNode>();
                    _ = node.TrySetValue("Labels", new ReadOnlyCollection<string>(v.Labels.ToList()));
                    return KeyValuePair.Create(v, node);
                })
                .ToDictionary();

            var nodes = dictNodes.Select(x => x.Value);

            var relationships = list
                .SelectMany(e => e.Values)
                .Where(v => v.Key == "relationship")
                .Select(v => v.Value)
                .SelectMany(v => v.As<List<IRelationship>>())
                .DistinctBy(v => v.ElementId)
                .Select(v =>
                {
                    var relationship = v.Properties.ToObject<TBranch>();
                    _ = relationship.TrySetValue("Type", v.Type);
                    var fromNode = ormNodes.First(n => n.ElementId == v.StartNodeElementId);
                    _ = relationship.TrySetValue("FromId", dictNodes[fromNode].GetValue<Guid>("Id"));
                    var toNode = ormNodes.First(n => n.ElementId == v.EndNodeElementId);
                    _ = relationship.TrySetValue("ToId", dictNodes[toNode].GetValue<Guid>("Id"));
                    return relationship;
                });

            return (nodes, relationships);
        });

        return new MatchByLabelResponse<TNode, TBranch>
        {
            Nodes = nodes.ToList(),
            Relationships = relationships.ToList()
        };
    }

    private static Dictionary<string, object?> GetProperties<TValue>(TValue value)
    {
        if (value is null)
        {
            return new Dictionary<string, object?>();
        }

        if (value is Dictionary<string, object?> dict)
        {
            return dict
                .ToDictionary(
                    pair => pair.Key,
                    pair => ConvertValue(pair.Value));
        }

        return value
            .GetType()
            .GetProperties()
            .ToDictionary(
                property => property.Name,
                property => ConvertValue(property.GetValue(value)));

        object? ConvertValue(object? val)
        {
            if (val is Guid guid)
            {
                return guid.ToString();
            }

            return val;
        }
    }

    private static string ConvertToStringProperties<TValue>(TValue value)
    {
        var properties = GetProperties(value);

        var convertedProperties = properties
            .Select(prop =>
            {
                if (prop.Value is string)
                {
                    return $"{prop.Key}: '{prop.Value}'";
                }

                return $"{prop.Key}: {prop.Value}";
            })
            .Join(",\n");

        return $$"""
                 {
                   {{convertedProperties}}
                 }
                 """;
    }
}