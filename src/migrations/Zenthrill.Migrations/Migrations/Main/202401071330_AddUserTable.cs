namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2024, 01, 07, 13, 30)]
public sealed class AddUserTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("Users")
            .WithColumn("Id")
                .AsGuid()
                .PrimaryKey()
            .WithColumn("Nickname")
                .AsString()
                .Indexed();
    }
}