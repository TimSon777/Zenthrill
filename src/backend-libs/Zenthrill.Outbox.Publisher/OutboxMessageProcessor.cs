using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Zenthrill.Outbox.Core;
using Zenthrill.Outbox.EntityFrameworkCore;
using Zenthrill.Providers;

namespace Zenthrill.Outbox.Publisher;

public interface IOutboxMessageProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

public sealed class OutboxMessageProcessor(
    IDbContextFactory<OutboxDbContext> dbContextFactory,
    IOutboxMessageBrokerProcessor outboxMessageBrokerProcessor,
    IDateTimeOffsetProvider dateTimeOffsetProvider,
    ILogger<OutboxMessageProcessor> logger) : IOutboxMessageProcessor
{
    private const string Query = """
                                 SELECT * FROM "OutboxMessages"
                                 WHERE "ScheduledAt" < NOW() AND "ProcessedAt" IS NULL
                                 FOR UPDATE SKIP LOCKED
                                 LIMIT 1
                                 """;

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            OutboxMessage? outboxMessage = null;

            IDbContextTransaction? transaction = null;
            try
            {
                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                transaction = await dbContext.Database
                    .BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                outboxMessage = await dbContext.OutboxMessages.FromSqlRaw(Query)
                    .FirstOrDefaultAsync(cancellationToken);

                if (outboxMessage is null)
                {
                    await Task.Delay(3000, cancellationToken);
                    continue;
                }

                await outboxMessageBrokerProcessor.ProcessAsync(outboxMessage);

                outboxMessage.ProcessedAt = dateTimeOffsetProvider.UtcNow;

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (transaction is not null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }

                if (outboxMessage is not null)
                {
                    logger.LogError(ex, "Exception occured when process outbox message with id {Id}", outboxMessage.Id);
                }
                else
                {
                    logger.LogError(ex, "Exception occured when process outbox message");
                }
            }
            finally
            {
                if (transaction is not null)
                {
                    await transaction.DisposeAsync();
                }
            }
        }
    }
}