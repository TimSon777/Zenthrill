namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2024, 01, 08, 20, 20)]
public sealed class AddOutboxMessageTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("OutboxMessages")
            .WithColumn("Id")
                .AsGuid()
                .PrimaryKey()
            .WithColumn("ScheduledAt")
                .AsDateTimeOffset()
                .Nullable()
            .WithColumn("ProcessedAt")
                .AsDateTimeOffset()
                .Nullable()
            .WithColumn("Data")
                .AsString()
            .WithColumn("Type")
                .AsString();
        
        Execute.Sql("""
                    CREATE INDEX IX_OutboxMessages_ScheduledAt_ProcessedAt
                    ON "OutboxMessages" ("ScheduledAt", "ProcessedAt")
                    """);
    }
}