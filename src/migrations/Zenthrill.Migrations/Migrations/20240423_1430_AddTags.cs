namespace Zenthrill.Migrations.Migrations;

[TimestampedMigration(2024, 04, 24, 14, 30)]
public sealed class AddTags : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("Tags")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString().NotNullable().Unique();
        
        Create.Table("StoryInfosTags")
            .WithColumn("StoryInfoId").AsGuid().ForeignKey("StoryInfos", "Id").NotNullable()
            .WithColumn("TagId").AsGuid().ForeignKey("Tags", "Id").NotNullable();

        Create.PrimaryKey("PK_StoryTags")
            .OnTable("StoryInfosTags")
            .Columns("StoryInfoId", "TagId");
        
        InsertTags(
            "Университеты",
            "Реклама",
            "Книги",
            "Магазины",
            "Здоровье",
            "Медицина",
            "Технологии",
            "Путешествия",
            "Образование",
            "Кулинария",
            "Искусство",
            "Спорт",
            "Музыка",
            "Кино",
            "Личное развитие",
            "Наука",
            "Финансы",
            "Юриспруденция",
            "Автомобили",
            "Политика",
            "Природа",
            "Мода",
            "Интерьер",
            "Литература",
            "История",
            "Фотография",
            "События",
            "Игры",
            "Хобби",
            "Домашние животные",
            "Трудоустройство",
            "Психология",
            "Фитнес");
        
    }

    private void InsertTags(params string[] names)
    {
        var syntax = Insert.IntoTable("Tags");

        foreach (var name in names)
        {
            syntax.Row(new { Id = Guid.NewGuid(), Name = name });
        }
    }
}