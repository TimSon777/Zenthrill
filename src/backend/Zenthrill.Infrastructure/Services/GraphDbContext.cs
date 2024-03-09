using Neo4jClient;
using Neo4jClient.Cypher;
using Zenthrill.Application;
using Zenthrill.Application.Repositories;

namespace Zenthrill.Infrastructure.Services;

public sealed class GraphDbContext(
    IRepositoryRegistry repositoryRegistry,
    BoltGraphClient boltGraphClient) : IGraphDbContext
{
    public async Task<T> ExecuteAsync<T>(Func<IRepositoryRegistry, ICypherFluentQuery, CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        using var transaction = boltGraphClient.BeginTransaction();
        var result = await action(repositoryRegistry, boltGraphClient.Cypher, cancellationToken);
        await transaction.CommitAsync();
        return result;
    }

    public async Task ExecuteAsync(Func<IRepositoryRegistry, ICypherFluentQuery, CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        using var transaction = boltGraphClient.BeginTransaction();
        await action(repositoryRegistry, boltGraphClient.Cypher, cancellationToken);
        await transaction.CommitAsync();
    }

    public async Task<T> ExecuteAsync<T>(Func<IRepositoryRegistry, CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        using var transaction = boltGraphClient.BeginTransaction();
        var result = await action(repositoryRegistry, cancellationToken);
        await transaction.CommitAsync();
        boltGraphClient.EndTransaction();
        return result;
    }

    public async Task ExecuteAsync(Func<IRepositoryRegistry, CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        using var transaction = boltGraphClient.BeginTransaction();
        await action(repositoryRegistry, cancellationToken);
        await transaction.CommitAsync();
        boltGraphClient.EndTransaction();
    }

    public async Task ExecuteAsync(Action<IRepositoryRegistry> action, CancellationToken _)
    {
        using var transaction = boltGraphClient.BeginTransaction();
        action(repositoryRegistry);
        await transaction.CommitAsync();
        boltGraphClient.EndTransaction();
    }

    public async Task<T> ReadAsync<T>(Func<IRepositoryRegistry, CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        return await action(repositoryRegistry, cancellationToken);
    }
}