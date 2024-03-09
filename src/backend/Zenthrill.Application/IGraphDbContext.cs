using Neo4jClient.Cypher;
using Zenthrill.Application.Repositories;

namespace Zenthrill.Application;

public interface IGraphDbContext
{
    Task<T> ExecuteAsync<T>(Func<IRepositoryRegistry, ICypherFluentQuery, CancellationToken, Task<T>> action, CancellationToken cancellationToken);

    Task<T> ExecuteAsync<T>(Func<IRepositoryRegistry, CancellationToken, Task<T>> action, CancellationToken cancellationToken);

    Task ExecuteAsync(Func<IRepositoryRegistry, CancellationToken, Task> action, CancellationToken cancellationToken);

    Task ExecuteAsync(Action<IRepositoryRegistry> action, CancellationToken cancellationToken);

    Task<T> ReadAsync<T>(Func<IRepositoryRegistry, CancellationToken, Task<T>> action, CancellationToken cancellationToken);
}