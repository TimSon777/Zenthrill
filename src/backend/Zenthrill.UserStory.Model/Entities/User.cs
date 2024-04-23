namespace Zenthrill.UserStory.Model.Entities;

public sealed class User
{
    public Guid Id { get; set; }

    public User(Guid id)
    {
        Id = id;
    }

    public User()
    {
        Id = Guid.NewGuid();
    }

    public required bool IsAnonymous { get; set; }
    
    public required string Agent { get; set; }

    public required ICollection<Story> Stories { get; set; } = default!;
}