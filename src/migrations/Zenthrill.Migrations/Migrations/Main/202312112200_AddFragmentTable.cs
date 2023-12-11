namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2023, 12, 11, 22, 00)]
public sealed class AddFragmentTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("Fragment")
            .WithColumn("Id")
                .AsGuid()
                .PrimaryKey()
            .WithColumn("Body")
                .AsString();
    }
}