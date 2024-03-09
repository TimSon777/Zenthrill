namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2024, 01, 07, 13, 35)]
public sealed class AddStoryInfoTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("StoryInfos")
            .WithColumn("Id")
                .AsGuid()
                .PrimaryKey()
            .WithColumn("Version")
                .AsString()
            .WithColumn("CreatorId")
                .AsGuid()
                .ForeignKey("Users", "Id")
            .WithColumn("EntrypointFragmentId")
                .AsGuid()
                .Nullable()
            .WithColumn("StoryName")
                .AsString();
    }
}