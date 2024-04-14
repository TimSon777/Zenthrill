using Microsoft.EntityFrameworkCore;
using Zenthrill.Domain.Entities;
using Zenthrill.Outbox;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Application;

public interface IApplicationDbContext
{
    DbSet<StoryInfo> StoryInfos { get; }
    
    DbSet<OutboxMessage> OutboxMessages { get; }

    DbSet<StoryInfoVersion> StoryInfoVersions { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}