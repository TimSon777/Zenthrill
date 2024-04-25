using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Zenthrill.UserStory.Model.Entities;
using Zenthrill.UserStory.Model.Infrastructure.EntityFrameworkCore;

namespace Zenthrill.UserStory.Model.Services;

public interface IUserProcessor
{
    Task<User> ProcessUserAsync(ClaimsPrincipal? claimsPrincipal, string agent, CancellationToken cancellationToken);
}

public sealed class UserProcessor(ApplicationDbContext applicationDbContext) : IUserProcessor
{
    public async Task<User> ProcessUserAsync(ClaimsPrincipal? claimsPrincipal, string agent, CancellationToken cancellationToken)
    {
        var existedUser = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Agent.Contains(agent), cancellationToken);

        if (existedUser is not null)
        {
            return existedUser;
        }

        var user = new User
        {
            Agent = agent,
            IsAnonymous = true,
            Stories = [],
        };

        await applicationDbContext.Users.AddAsync(user, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}