namespace Zenthrill.Auth.WebAPI.Responses;

public sealed class UserResponse
{
    public required Guid Id { get; set; }
    
    public required string UserName { get; set; }
    
    public required IEnumerable<string> Roles { get; set; }
}