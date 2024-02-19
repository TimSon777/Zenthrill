using StronglyTypedIds;
using Zenthrill.Domain.Common;

namespace Zenthrill.Domain.Entities;

[StronglyTypedId(Template.Guid)]
public partial struct UserId;

public sealed class User : Entity<UserId>
{
    public required string Nickname { get; set; }

    public User()
    {
        Id = UserId.New();
    }

    public bool HasAccessToRead(StoryInfo _)
    {
        return true;
    }

    public bool HasAccessToUpdate(StoryInfo storyInfo)
    {
        return this == storyInfo.Creator;
    }
}