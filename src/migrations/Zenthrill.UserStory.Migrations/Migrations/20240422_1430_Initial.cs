namespace Zenthrill.UserStory.Migrations.Migrations;

[TimestampedMigration(2024, 04, 22, 14, 30)]
public sealed class Initial : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("IsAnonymous").AsBoolean().NotNullable()
            .WithColumn("Agent").AsString().NotNullable();

        Create
            .Table("Stories")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("UserId").AsGuid().ForeignKey("Users", "Id").Indexed()
            .WithColumn("ExecutionContext").AsString().NotNullable()
            .WithColumn("StoryInfoVersionId").AsGuid().NotNullable();
    }
}