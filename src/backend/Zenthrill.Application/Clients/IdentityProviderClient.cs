using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Zenthrill.Application.Settings;
using Zenthrill.Domain.Entities;

namespace Zenthrill.Application.Clients;

public interface IIdentityProviderClient
{
    Task<User> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken);
}

public sealed class IdentityProviderClient(
    IHttpClientFactory httpClientFactory,
    IOptions<IdentityProviderSettings> identityProviderSettings) : IIdentityProviderClient
{
    private readonly IdentityProviderSettings _identityProviderSettings = identityProviderSettings.Value;

    public async Task<User> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        var uri = _identityProviderSettings.GetUserUri(userId);

        var userDto = await client.GetFromJsonAsync<UserDto>(uri, cancellationToken);

        if (userDto is null)
        {
            throw new InvalidOperationException($"Can,t retrieve user from identity provider with id = {userId.Value}");
        }

        return new User
        {
            Id = new UserId(userDto.Id),
            UserName = userDto.UserName,
            Roles = userDto.Roles.ToList()
        };
    }
}

file sealed class UserDto
{
    public required Guid Id { get; set; }
    
    public required IEnumerable<string> Roles { get; set; }
    
    public required string UserName { get; set; }
}