namespace Zenthrill.Migrations.Migrations.Main;

[TimestampedMigration(2024, 01, 07, 13, 35)]
public sealed class AddStoryTables : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("StoryInfos")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CreatorId").AsGuid().NotNullable().Indexed()
            .WithColumn("Description").AsString().NotNullable();

        Create
            .Table("StoryInfoVersions")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("StoryInfoId").AsGuid().ForeignKey("StoryInfos", "Id").Indexed()
            .WithColumn("Version").AsString().NotNullable()
            .WithColumn("Name").AsString()
            .WithColumn("EntrypointFragmentId").AsGuid().Nullable()
            .WithColumn("BaseVersionId").AsGuid().Nullable().ForeignKey("StoryInfoVersions", "Id")
            .WithColumn("IsPublished").AsBoolean().NotNullable();
    }
}