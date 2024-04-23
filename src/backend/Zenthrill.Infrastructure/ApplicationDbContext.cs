using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zenthrill.Application;
using Zenthrill.Domain.Entities;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<StoryInfo> StoryInfos => Set<StoryInfo>();

    public DbSet<StoryInfoVersion> StoryInfoVersions => Set<StoryInfoVersion>();
    
    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public new async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
    }
}