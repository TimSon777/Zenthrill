using System.Security.Claims;

namespace Zenthrill.WebAPI.Common;

public interface IUserMapper
{
    User MapToApplicationUser(ClaimsPrincipal principal);
}

public sealed class UserMapper : IUserMapper
{
    public User MapToApplicationUser(ClaimsPrincipal principal)
    {
        return new User
        {
            Id = new UserId(),
            Roles = new List<string>()
        };
    }
}

