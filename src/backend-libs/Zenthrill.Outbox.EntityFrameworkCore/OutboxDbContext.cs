using Microsoft.EntityFrameworkCore;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Outbox.EntityFrameworkCore;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; }
}

public sealed class OutboxDbContext(DbContextOptions<OutboxDbContext> options)
    : DbContext(options), IOutboxDbContext
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
}