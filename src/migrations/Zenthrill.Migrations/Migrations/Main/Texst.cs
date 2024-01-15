namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2024, 1, 1, 12, 00)]
public class Texst : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("BadUsers")
            .WithColumn("Id")
            .AsGuid()
            .PrimaryKey();

        Execute.Sql("CREATE EXTENSION \"uuid-ossp\";");
        Create
            .Column("BadUserId")
            .OnTable("StoryInfos")
            .AsGuid().ForeignKey().SetExistingRowsTo(SystemMethods.NewGuid);
    }
}